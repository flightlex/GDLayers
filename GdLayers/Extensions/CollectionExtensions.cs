using System;
using System.Collections.Generic;
using System.Linq;

namespace GdLayers.Extensions;

public static class CollectionExtensions
{
    public static List<IEnumerable<T>> SplitIntoChunks<T>(this ICollection<T> source, int chunkSize)
    {
        var result = new List<IEnumerable<T>>();
        var sourceList = source.ToList();

        for (int i = 0; i < sourceList.Count; i += chunkSize)
        {
            var chunk = sourceList.Skip(i).Take(chunkSize).ToArray();
            result.Add(chunk);
        }

        return result;
    }
}
