using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.IO;

namespace SeadQueryCore
{
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

        public static void InsertAt<T>(this List<T> array, T itemToFind, T itemToInsert)
        {
            var idx = array.IndexOf(itemToFind);

            if (idx < 0)
                throw new ArgumentException($"List<T>.InsertAt: {itemToFind} to found");

            array.Insert(idx, itemToInsert);
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

    public static class Utility {

        public static bool empty(string x) => (x ?? "").Equals("");

        public static string Coalesce(params string[] data)
        {
            return data.FirstOrDefault(x => !x.IsEmpty()) ?? "";
        }

        public static List<T> ToList<T>(params T[] items) => new List<T>(items);

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
