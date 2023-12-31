using System.Text.RegularExpressions;

namespace Tazor.Extensions;

public static partial class UrlExtensions
{
    [GeneratedRegex("{([^:}]*):?([^}]*)}")]
    private static partial Regex TokenRegex();

    public static bool HasParameters(this string template)
    {
        return TokenRegex().Matches(template).Count != 0;
    }
}