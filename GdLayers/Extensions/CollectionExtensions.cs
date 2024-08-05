using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

    public static bool HasDuplicates<T, TKey>(this IEnumerable<T> source, Func<T, TKey> predicate)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));
        if (predicate == null) throw new ArgumentNullException(nameof(predicate));

        var seenKeys = new HashSet<TKey>();
        foreach (var item in source)
        {
            var key = predicate(item);
            if (!seenKeys.Add(key))
            {
                return true; // Duplicate found
            }
        }
        return false; // No duplicates found
    }

    public static void ParallelForEach<T>(this ICollection<T> collection, Action<T> action)
    {
        collection.AsParallel().ForAll(action);
    }

    public static void PartitionedParallelForEach<T>(this ICollection<T> collection, Action<T> action)
    {
        var partitioner = Partitioner.Create<T>(collection);
        Parallel.ForEach(partitioner, action);
    }

    public static void ForEach<T>(this ICollection<T> collection, Action<T> action)
    {
        foreach (var item in collection)
            action(item);
    }
}
