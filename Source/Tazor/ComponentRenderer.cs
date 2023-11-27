using System.Reflection;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Tazor;

public class ComponentRenderer
{
    private readonly HtmlRenderer _htmlRenderer;

    public ComponentRenderer()
    {
        var services = new ServiceCollection().AddLogging();
        var serviceProvider = services.BuildServiceProvider();
        var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
        
        _htmlRenderer = new HtmlRenderer(serviceProvider, loggerFactory);
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