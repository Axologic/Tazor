using Microsoft.AspNetCore.Components;

namespace Tazor;

public interface IParameterizedView
{
    static abstract IEnumerable<ParameterView> GetParameters();
}