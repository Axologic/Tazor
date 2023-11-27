using System.Reflection;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Tazor;

public class RazorComponentsResolver : IDocumentResolver
{
    private readonly HtmlRenderer _htmlRenderer;
    
    public RazorComponentsResolver()
    {
        var services = new ServiceCollection().AddLogging();
        var serviceProvider = services.BuildServiceProvider();
        var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
        
        _htmlRenderer = new HtmlRenderer(serviceProvider, loggerFactory);
    }

    public async Task<Document[]> GetDocuments()
    {
        var components = Assembly.GetEntryAssembly()!.GetTypes()
            .Where(t => t.IsAssignableTo(typeof(ComponentBase)))
            .Where(t => t.GetCustomAttributes<RouteAttribute>().Any());

        var results = new List<Document>();
        
        foreach (var component in components)
        {
            var componentDocuments = await GetDocuments(component);
            results.AddRange(componentDocuments);
        }

        return results.ToArray();
    }
    
    private async Task<IEnumerable<Document>> GetDocuments(Type componentType)
    {
        var results = new List<Document>();
        
        foreach (var template in componentType.GetCustomAttributes<RouteAttribute>().Select(a => a.Template))
        {
            var templateTokens = UrlExtensions.GetTokens(template);
            
            if (template.HasParameters())
            {
                var isParameterizedView = componentType.GetInterfaces()
                    .Where(i => i.IsGenericType)
                    .Any(i => i.GetGenericTypeDefinition().IsAssignableTo(typeof(IParameterizedView<>)));

                if (isParameterizedView is false)
                {
                    throw new InvalidOperationException();
                }
                
                var method = componentType.GetMethod(nameof(IParameterizedView<object>.GetData), BindingFlags.Public | BindingFlags.Static);
                var data = (IEnumerable<object>) method!.Invoke(null, null)!;

                foreach (var item in data)
                {
                    var dictionary = item.GetType()
                        .GetProperties()
                        .Where(x => templateTokens.Any(t => t.Equals($"{{{x.Name}}}", StringComparison.OrdinalIgnoreCase)))
                        .ToDictionary(p => p.Name, p => p.GetValue(item));
                    
                    var parameters = ParameterView.FromDictionary(dictionary);

                    var html = await GetHtml(componentType, parameters);
                    var url = template;
                    foreach (var parameter in parameters)
                    {
                        url = url.Replace($"{{{parameter.Name}}}", parameter.Value.ToString(), StringComparison.InvariantCultureIgnoreCase);
                    }
                    
                    results.Add(new Document
                    {
                        Url = url,
                        Html = html
                    });
                }
            }
            else
            {
                var html = await GetHtml(componentType, ParameterView.Empty);
                results.Add(new Document
                {
                    Url = template,
                    Html = html
                });
            }
        }

        return results;
    }
    
    public async Task<string> GetHtml(Type componentType, ParameterView parameters)
    {
        var layout = componentType.GetCustomAttribute<LayoutAttribute>()?.LayoutType;
        string html;
        if (layout is null)
        {
            html = await _htmlRenderer.Dispatcher.InvokeAsync(async () =>
            {
                var output = await _htmlRenderer.RenderComponentAsync(componentType, parameters);
                return output.ToHtmlString();
            });
        }
        else
        {
            html = await _htmlRenderer.Dispatcher.InvokeAsync(async () =>
            {
                var output = await _htmlRenderer.RenderComponentAsync(typeof(LayoutView), ParameterView.FromDictionary(
                    new Dictionary<string, object?>
                    {
                        { nameof(LayoutView.Layout), layout },
                        { nameof(LayoutView.ChildContent), (RenderFragment)ChildContent }
                    }));

                return output.ToHtmlString();

                void ChildContent(RenderTreeBuilder builder)
                {
                    builder.OpenComponent<DynamicComponent>(0);
                    builder.AddAttribute(1, nameof(DynamicComponent.Type), componentType);
                    builder.AddAttribute(2, nameof(DynamicComponent.Parameters), parameters.ToDictionary());
                    builder.CloseComponent();
                }
            });
        }

        return html;
    }
}