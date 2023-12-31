using Microsoft.AspNetCore.Components;

namespace Tazor.Services;

public interface IComponentRenderer
{
    Task<string> GetHtml(Type componentType, ParameterView parameters);
}