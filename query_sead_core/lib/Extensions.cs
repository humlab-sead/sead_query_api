using System;
using System.Collections.Generic;
using System.Linq;

namespace QueryFacetDomain
{
    public static class ForEachExt {
        public static void ForEach<T>(this IEnumerable<T> enumeration, Action<T> action)
        {
            foreach (T item in enumeration) {
                action(item);
            }
        }
    }
}
