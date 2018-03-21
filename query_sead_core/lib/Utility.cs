using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.IO;

namespace QuerySeadDomain {

    public static class MyListExtensions
    {

        public static List<string> AddIfMissing(this List<string> array, string element)
        {
            if (element != null && !array.Contains(element))
                array.Add(element);
            return array;
        }

        public static IEnumerable<string> AppendIf(this IEnumerable<string> array, string element)
        {
            return element.IsEmpty() ? array : array.Append(element);
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

        public static void MyInsertBeforeItem<T>(this List<T> array, T search_item, T insert_item)
        {
            var idx = array.FindIndex(z => search_item.Equals(z));
            if (idx >= 0)
                array.Insert(idx, insert_item);
            else
                throw new ArgumentException($"List<T>.InsertBeforeItem: {search_item} to found");
        }

        public static int MyFindClosestIndex<T>(this List<T> rows, string search_str, Func<T, string> selector)
        {
            search_str = search_str.ToLower();
            int pos = rows.FindIndex(z => selector(z).Substring(0, search_str.Length).ToLower().Equals(search_str));
            if (pos == -1)
            {
                pos = rows.MyFindClosestIndex2<T>(search_str, selector);
            }
            return pos;
        }

        /*
        Function:  findClosestIndex
        Parameters:
        * $rows of the particular facet
        * $search_str the text string to position the facet
        */

        private static int MyFindClosestIndex2<T>(this List<T> rows, string search_str, Func<T, string> selector)
        {
            int start_position = 0;
            search_str = search_str.ToLower();
            int end_position = rows.Count - 1;
            int position = (int)Math.Floor((end_position - start_position) / 2.0);
            while ((end_position - start_position) > 1)
            {
                var row = rows[position];
                var compare_to_str = selector(row).Substring(0, search_str.Length);
                var order = String.Compare(search_str, 0, compare_to_str, 0, search_str.Length, StringComparison.CurrentCultureIgnoreCase);
                if (order == 0)
                {
                    break;
                } else if (order < 0)
                {
                    end_position = position;
                    position = start_position + (int)Math.Floor((end_position - start_position) / 2.0);
                } else
                {
                    start_position = position;
                    position = start_position + (int)Math.Ceiling((end_position - start_position) / 2.0);
                }
            }
            return position;
        }

    }

    public static class StringUtilityExtension
    {
        public static bool IsEmpty(this string x) => (x ?? "").Equals("");

        public static string Prepend(this string text, string prefix, string glue = "", bool preserveEmpty = true)
        {
            return (preserveEmpty && text.IsEmpty()) ? "" : $"{prefix}{glue}{text}";
        }

        /// <summary>
        /// Returns "{prefix}{glue}{text}" if text is not empty, otherwise empty string
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="text"></param>
        /// <param name="glue"></param>
        /// <param name="preserveEmpty"></param>
        /// <returns></returns>
        public static string GlueTo(this string prefix, string text, string glue = "", bool preserveEmpty = true)
        {
            return (preserveEmpty && text.IsEmpty()) ? "" : $"{prefix}{glue}{text}";
        }

        /// <summary>
        /// Returns $"{text}{glue}{suffix}" if suffiz not empty, otherwise text
        /// </summary>
        /// <param name="text"></param>
        /// <param name="suffix"></param>
        /// <param name="glue"></param>
        /// <returns></returns>
        public static string GlueIf(this string text, string suffix, string glue = "")
        {
            return (suffix ?? "").IsEmpty() ? text : $"{text}{glue}{suffix}";
        }

    }

    public class Utility {

        public static bool empty(string x) => (x ?? "").Equals("");
        //public static bool empty(int? x) => x == null;
        //public static bool empty<T>(T x) => x == null;

        //public static string str_prefix(string prefix, string text, string glue = "")
        //{
        //    return empty(text) ? "" : $"{prefix}{glue}{text}";
        //}

        //public static string array_join_surround<T>(List<T> array, string glue = "", string prefix = "", string suffix = "", string default_value = "")
        //{
        //    return String.Join(glue, array.Select(x => $"{prefix}{x.ToString()??default_value}{suffix}").ToList());
        //}

        public static string Coalesce(params string[] data)
        {
            return data.FirstOrDefault(x => !x.IsEmpty()) ?? "";
        }

        public static List<T> ToList<T>(params T[] items) => new List<T>(items);

        //public T coalesce<T>(params T[] data)
        //{
        //    return data.FirstOrDefault(x => !empty(x));
        //}


        public static string ToJson(object value)
        {
            var resolver = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
            return JsonConvert.SerializeObject(value, Formatting.Indented, resolver);
        }

        public static void SaveAsJson(object value, string file_prefix, string logDir)
        {
            var timestamp = DateTime.Now.ToString("yyyyddM_HHmmss");
            var filename = Path.Combine(logDir, string.Format("{0}_{1}.json", file_prefix, timestamp));
            string data = Utility.ToJson(value);
            File.WriteAllText(filename, data);
        }
    }
}
