using System.Text.RegularExpressions;

namespace SeadQueryCore
{
    public static class StringExtension
    {
        public static bool IsEmpty(this string x) => (x ?? "").Equals("");
        public static bool IsNotEmpty(this string x) => ! (x ?? "").Equals("");

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

        /// <summary>
        /// Returnes a compressed string where all whitespace sequences are replaced with single spaces.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string Squeeze(this string text)
        {
            return Regex.Replace(text ?? "", @"\s+", " ").Trim();
        }
    }
}
