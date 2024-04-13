using System;
using System.Collections.Generic;
using System.Linq;

namespace SeadQueryCore
{
    public static class ListExtensions
    {
        public static List<string> AddIfMissing(this List<string> array, string element)
        {
            if (element != null && !array.Contains(element))
                array.Add(element);
            return array;
        }

        public static string Combine(this List<string> array, string glue)
        {
            return String.Join(glue, array);
        }

        public static string Combine<T>(this List<T> array, string glue, Func<T, string> selector)
        {
            return String.Join(glue, array.Select(selector).ToList());
        }

        public static string Combine<T>(this List<T> array, string glue = "", string prefix = "", string suffix = "", string default_value = "")
        {
            return String.Join(glue, array.Select(x => $"{prefix}{x.ToString() ?? default_value}{suffix}").ToList());
        }

        /// <summary>
        /// Inserts item itemToInsert at itemToFind's position i.e before itemToFind
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        /// <param name="itemToFind"></param>
        /// <param name="itemToInsert"></param>
        public static List<T> InsertAt<T>(this List<T> array, T itemToFind, T itemToInsert)
        {
            var idx = array.IndexOf(itemToFind);

            if (idx < 0)
                throw new ArgumentException($"List<T>.InsertAt: {itemToFind} not found");

            array.Insert(idx, itemToInsert);

            return array;
        }
    }
}
