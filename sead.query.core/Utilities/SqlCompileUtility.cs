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
            return (filter?.Length == 0) ? "" : $" AND {expr} ILIKE '{filter}' ";
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

        public static string BetweenExpr(string expr, List<decimal> bound)
        {
            decimal lower = bound[0];
            decimal upper = bound[1];
            var c = CultureInfo.GetCultureInfo("en-US");
            return (lower == upper) ? $" (floor({expr}) = {lower.ToString(c)})"
                : $" ({expr} >= {lower.ToString(c)} and {expr} <= {upper.ToString(c)})";
        }

        /// <summary>
        /// Returns a SQL expression that checks if a GIS coordinate is within a polygon using PostGIS UDF call.
        /// </summary>
        /// <param name="pointExpr">"ST_MakePoint(latitude_dd, longitude_dd)"</param>
        /// <param name="polygon"></param>
        /// <returns></returns>
        public static string WithinPolygonExpr(string pointExpr, List<decimal> polygon)
        {
            var c = CultureInfo.GetCultureInfo("en-US");
            if (polygon.Count % 2 != 0 || polygon.Count < 6)
                throw new ArgumentException("Invalid polygon sizes");
            // Create PostGIS points out of a list of polygon coordinates where coordinates are pairs of lat/long values
            var points = new List<string>();
            for (int i = 0; i < polygon.Count; i += 2)
                points.Add($"ST_MakePoint({polygon[i].ToString(c)}, {polygon[i + 1].ToString(c)})");
            // Create a PostGIS polygon out of the points
            var polygonExpr = $"ST_MakePolygon(ST_MakeLine(ARRAY[{String.Join(", ", points)}]))";
            // Return the PostGIS UDF call
            return $"ST_Within({pointExpr}, {polygonExpr})";
        }

        public static string WithinPolygonExpr(string latitude_column, string longitude_column, List<decimal> polygon)
        {
            var pointExpr = $"ST_MakePoint({latitude_column}, {longitude_column})";
            return WithinPolygonExpr(pointExpr, polygon);
        }

        /// <summary>
        /// Returns a SQL expression that checks if a numrangeexpr is within given buounds
        /// </summary>
        /// <param name="rangeExpr">numrange(age_younger, age_older, '[]')</param>
        /// <param name="rangeOperator"></param>
        /// <param name="rangeType"></param>
        /// <param name="bounds"></param>
        /// Create a integer version of this method
        public static string RangeExpr(string rangeExpr, string rangeType, string rangeOperator, List<int> bounds)
        {
            return $"{rangeType}({bounds[0]}, {bounds[1]}, '[]') {rangeOperator} {rangeExpr}";
        }

        public static string RangeExpr(string rangeExpr, string rangeType, string rangeOperator, List<decimal> bounds)
        {
            return $"{rangeType}({bounds[0].ToStringEn()}, {bounds[1].ToStringEn()}, '[]') {rangeOperator} {rangeExpr}";
        }
    }
}
