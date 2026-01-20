using System.Text.RegularExpressions;

namespace Agility.Net.Blazor.Starter.Util;

public static partial class Utility
{
    public static string CleanHtml(string html)
    {
        var pattern = @"href=""~\/";
        var replacement = "href=\"/";
        return Regex.Replace(html, pattern, replacement);
    }
}
