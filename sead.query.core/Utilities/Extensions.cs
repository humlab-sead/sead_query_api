using System;
using System.Collections.Generic;
using System.Linq;

namespace SeadQueryCore
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
