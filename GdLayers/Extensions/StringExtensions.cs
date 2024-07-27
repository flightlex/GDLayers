using System;

namespace GdLayers.Extensions;

public static class StringExtensions
{
    public static bool ContainsExt(this string str, string value, StringComparison comparison = StringComparison.OrdinalIgnoreCase)
    {
        return str.IndexOf(value, comparison) >= 0;
    }
}
