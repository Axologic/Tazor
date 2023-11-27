using System.Reflection;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Tazor;

public static class TazorGenerator
{
    private static readonly HtmlRenderer HtmlRenderer;

    static TazorGenerator()
    {
        var services = new ServiceCollection().AddLogging();
        var serviceProvider = services.BuildServiceProvider();
        var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
        
        HtmlRenderer = new HtmlRenderer(serviceProvider, loggerFactory);
    }

    public static async Task Generate()
    {
        var components = Assembly.GetEntryAssembly()!.GetTypes()
            .Where(t => t.IsAssignableTo(typeof(ComponentBase)))
            .Where(t => t.GetCustomAttribute<RouteAttribute>() is not null);
        
        foreach (var component in components)
        {
            await Generate(component);
        }
    }

    private static async Task Generate(Type componentType)
    {
        foreach (var template in componentType.GetCustomAttributes<RouteAttribute>().Select(a => a.Template))
        {
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
                    var dictionary = item.GetType().GetProperties().ToDictionary(p => p.Name, p => p.GetValue(item));
                    var parameters = ParameterView.FromDictionary(dictionary);

                    var html = await GetHtml(componentType, parameters);
                    var url = template;
                    foreach (var parameter in parameters)
                    {
                        url = url.Replace($"{{{parameter.Name}}}", parameter.Value.ToString(), StringComparison.InvariantCultureIgnoreCase);
                    }

                    await WriteFile(url, html);
                }
            }
            else
            {
                var html = await GetHtml(componentType, ParameterView.Empty);
                await WriteFile(template, html);
            }
        }
    }

    private static async Task WriteFile(string url, string html)
    {
        url = url.TrimStart('/');
        
        if (string.IsNullOrWhiteSpace(url))
        {
            url = "index";
        }
        
        var directory = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!, "Output");
        var outputPath = Path.Combine(directory, $"{url}.html");
            
        Directory.CreateDirectory(Path.GetDirectoryName(outputPath)!);
        
        await File.WriteAllTextAsync(outputPath, html);
    }

    private static async Task<string> GetHtml(Type componentType, ParameterView parameters)
    {
        var layout = componentType.GetCustomAttribute<LayoutAttribute>()?.LayoutType;
        string html;
        if (layout is null)
        {
            html = await HtmlRenderer.Dispatcher.InvokeAsync(async () =>
            {
                var output = await HtmlRenderer.RenderComponentAsync(componentType, parameters);
                return output.ToHtmlString();
            });
        }
        else
        {
            html = await HtmlRenderer.Dispatcher.InvokeAsync(async () =>
            {
                var output = await HtmlRenderer.RenderComponentAsync(typeof(LayoutView), ParameterView.FromDictionary(
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