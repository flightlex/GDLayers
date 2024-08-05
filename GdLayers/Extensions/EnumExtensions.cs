using System;
using System.Collections.Generic;
using System.Linq;

namespace GdLayers.Extensions;

public static class EnumExtensions
{
    public static TAttribute? GetAttribute<TAttribute>(this Enum enumVal) where TAttribute : Attribute
    {
        var collection = GetAttributes<TAttribute>(enumVal);
        return collection.Count() > 0 ? collection.ElementAt(0) : null;
    }
    public static IEnumerable<TAttribute?> GetAttributes<TAttribute>(this Enum enumVal) where TAttribute : Attribute
    {
        var type = enumVal.GetType();
        var memInfo = type.GetMember(enumVal.ToString());
        var attributes = memInfo[0].GetCustomAttributes(typeof(TAttribute), false);

        return (attributes.Length > 0) ? (IEnumerable<TAttribute?>)attributes : [];
    }

}
