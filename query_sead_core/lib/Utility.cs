using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace QueryFacetDomain {

    public class Utility {

        public static bool empty(string x) => (x ?? "").Equals("");
        //public static bool empty(int? x) => x == null;
        //public static bool empty<T>(T x) => x == null;

        public static string str_prefix(string prefix, string text, string glue = "")
        {
            return empty(text) ? "" : $"{prefix}{glue}{text}";
        }

        public static string array_join_surround<T>(List<T> array, string glue = "", string prefix = "", string suffix = "", string default_value = "")
        {
            return String.Join(glue, array.Select(x => $"{prefix}{x.ToString()??default_value}{suffix}").ToList());
        }

        public static string Coalesce(params string[] data)
        {
            return data.FirstOrDefault(x => !empty(x)) ?? "";
        }

        public static List<T> ToList<T>(params T[] items) => new List<T>(items);

        //public T coalesce<T>(params T[] data)
        //{
        //    return data.FirstOrDefault(x => !empty(x));
        //}
        public static List<T> array_insert_before_existing<T>(List<T> array, T search_item, T insert_item)
        {
            var idx = array.FindIndex(z => search_item.Equals(z));
            if (idx >= 0)
                array.Insert(idx, insert_item);
            return array;
        }

        public static int array_find_index<T>(List<T> rows, string search_str, Func<T,string> selector)
        {
            search_str = search_str.ToLower();
            int pos = rows.FindIndex(z => selector(z).Substring(0, search_str.Length).ToLower().Equals(search_str));
            if (pos == -1) {
                pos = array_find_closest_index(rows, search_str, selector);
            }
            return pos;
        }
        
        /*
        Function:  findClosestIndex
        Parameters:
        * $rows of the particular facet
        * $search_str the text string to position the facet
        */

        private static int array_find_closest_index<T>(List<T> rows, string search_str, Func<T, string> selector)
        {
            int start_position = 0;
            search_str = search_str.ToLower();
            int end_position = rows.Count - 1;
            int position = (int)Math.Floor((end_position - start_position) / 2.0);
            while ((end_position - start_position) > 1) {
                var row = rows[position];
                var compare_to_str = selector(row).Substring(0, search_str.Length);
                var order = String.Compare(search_str, 0, compare_to_str, 0, search_str.Length, StringComparison.CurrentCultureIgnoreCase);
                if (order == 0) {
                    break;
                } else if (order < 0) {
                    end_position = position;
                    position = start_position + (int)Math.Floor((end_position - start_position) / 2.0);
                } else {
                    start_position = position;
                    position = start_position + (int)Math.Ceiling((end_position - start_position) / 2.0);
                }
            }
            return position;
        }

    }
}
