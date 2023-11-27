using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Components;

namespace Tazor;

public static partial class UrlExtensions
{
    public static bool HasParameters(this string template)
    {
        return TokenRegex().Matches(template).Count != 0;
    }
    
    public static ParameterView GetParameters(string url, string template)
    {
        var tokens = TokenRegex().Matches(template);
        var result = new Dictionary<string, object?>();
        foreach (Match token in tokens)
        {
            var tokenName = token.Captures.First();
            var tokenValue = url.Substring(token.Index, token.Length);
            result.Add(tokenName.Value, tokenValue);
        }

        return ParameterView.FromDictionary(result);
    }

    public static string GetUrl(ParameterView parameters, string template)
    {
        foreach (var parameter in parameters)
        {
            template = template.Replace($"{{{parameter.Name}}}", parameter.Value.ToString(), StringComparison.InvariantCultureIgnoreCase);
        }

        return template;
    }

    [GeneratedRegex("{([^:}]*):?([^}]*)}")]
    private static partial Regex TokenRegex();
}