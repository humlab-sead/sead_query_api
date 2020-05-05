using System;
using System.Collections.Generic;
using System.Linq;

namespace SeadQueryCore
{
    public static class IEnumerableExtensions
    {

        public static IEnumerable<T> AddUnion<T>(this IEnumerable<T> values, T value)
            => value == null ? values : values.Union(new List<T> { value });

        public static IEnumerable<T> NullableUnion<T>(this IEnumerable<T> values, IEnumerable<T> moreValues)
            => moreValues == null ? values : values.Union(moreValues);

        public static void ForEach<T>(this IEnumerable<T> enumeration, Action<T> action)
        {
            foreach (T item in enumeration) {
                action(item);
            }
        }

        public static IEnumerable<TResult> PairWise<TSource, TResult>(
            this IEnumerable<TSource> source,
            Func<TSource, TSource, TResult> selector)
        {
            return Enumerable.Zip(source, source.Skip(1), selector);
        }
    }
}
