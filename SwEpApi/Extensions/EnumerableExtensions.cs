using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SwEpApi.Extensions
{
    public static class EnumerableExtensions
    {
        // Adapted from https://blogs.msdn.microsoft.com/pfxteam/2012/03/05/implementing-a-simple-foreachasync-part-2/
        public static Task ForEachAsync<T>(this IEnumerable<T> source, int degreeOfParallelism, Func<T, Task> body, IProgress<T> progress = null)
        {
            return Task.WhenAll(
                Partitioner.Create(source).GetPartitions(degreeOfParallelism)
                    .Select(partition => Task.Run(async () => {
                        using (partition)
                            while (partition.MoveNext())
                            {
                                await body(partition.Current);
                                progress?.Report(partition.Current);
                            }
                    }))
            );
        }

        public static IEnumerable<List<T>> SplitList<T>(this List<T> bigList, int nSize = 3)
        {
            for (int i = 0; i < bigList.Count; i += nSize)
            {
                yield return bigList.GetRange(i, Math.Min(nSize, bigList.Count - i));
            }
        }

        public static IEnumerable<IEnumerable<T>> SplitIntoSets<T> (this IEnumerable<T> source, int itemsPerSet)
        {
            var sourceList = source as List<T> ?? source.ToList();
            for (var index = 0; index < sourceList.Count; index += itemsPerSet)
            {
                yield return sourceList.Skip(index).Take(itemsPerSet);
            }
        }

    }
}
