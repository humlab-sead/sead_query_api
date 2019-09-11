using System;
using System.Collections.Generic;

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
            return InExpr(expr, values.ConvertAll(z => (int)z));
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
            return (lower == upper) ? $" (floor({expr}) = {lower})" : $" ({expr} >= {lower} and {expr} <= {upper})";
        }
    }
}