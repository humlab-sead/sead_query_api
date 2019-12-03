using System;
using System.Collections.Generic;
using System.Globalization;

namespace SeadQueryCore
{
    public static class SqlCompileUtility
    {
        // LikeExpr(Facet.CategoryNameExpr, TextFilter)
        public static string LikeExpr(string expr, string filter)
        {
            return (filter == "") ? "" : $" AND {expr} ILIKE '{filter}' ";
        }

        public static string InExpr(string expr, List<decimal> values)
        {
            var c = CultureInfo.GetCultureInfo("en-US");
            return InExpr(expr, values.ConvertAll(z => z.ToString(c)));
        }

        public static string InExpr(string expr, List<int> values)
        {
            return $" ({expr}::int in (" + String.Join(", ", values) + ")) ";
        }

        public static string InExpr(string expr, List<string> values)
        {
            return $" ({expr}::text in (" + String.Join(", ", values.ConvertAll(z => $"'{z}'")) + ")) ";
        }

        public static string BetweenExpr(string expr, decimal lower, decimal upper)
        {
            var c = CultureInfo.GetCultureInfo("en-US");
            return (lower == upper) ? $" (floor({expr}) = {lower.ToString(c)})"
                : $" ({expr} >= {lower.ToString(c)} and {expr} <= {upper.ToString(c)})";
        }

    }
}