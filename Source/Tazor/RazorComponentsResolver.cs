using System.Reflection;
using Microsoft.AspNetCore.Components;

namespace Tazor;

public class RazorComponentsResolver : IDocumentResolver
{
    private readonly ComponentRenderer _componentRenderer = new();

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
            if (template.HasParameters())
            {
                var isParameterizedView = componentType.IsAssignableTo(typeof(IParameterizedView));
                if (isParameterizedView is false)
                {
                    throw new InvalidOperationException();
                }
                
                var method = componentType.GetMethod(nameof(IParameterizedView.GetParameters), BindingFlags.Public | BindingFlags.Static);
                var parameterViews = (IEnumerable<ParameterView>) method!.Invoke(null, null)!;

                foreach (var parameters in parameterViews)
                {
                    var html = await _componentRenderer.GetHtml(componentType, parameters);
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
                var html = await _componentRenderer.GetHtml(componentType, ParameterView.Empty);
                results.Add(new Document
                {
                    Url = template,
                    Html = html
                });
            }
        }

        return results;
    }
}