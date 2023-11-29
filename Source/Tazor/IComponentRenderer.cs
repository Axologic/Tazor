using Microsoft.AspNetCore.Components;

namespace Tazor;

public interface IComponentRenderer
{
    Task<string> GetHtml(Type componentType, ParameterView parameters);
}