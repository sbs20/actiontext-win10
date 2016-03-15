using System.Collections.Generic;

namespace Sbs20.Actiontext.Extensions
{
    public static class ICollectionExtension
    {
        public static void Add<TSource>(this ICollection<TSource> first, IEnumerable<TSource> second)
        {
            foreach (TSource item in second)
            {
                first.Add(item);
            }
        }
    }
}