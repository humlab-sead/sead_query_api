using System;
using System.Collections.Generic;
using System.Globalization;
using System.Xml.Schema;

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

        public static string BetweenExpr(string expr, decimal lower, decimal upper)
        {
            var c = CultureInfo.GetCultureInfo("en-US");
            return (lower == upper) ? $" (floor({expr}) = {lower.ToString(c)})"
                : $" ({expr} >= {lower.ToString(c)} and {expr} <= {upper.ToString(c)})";
        }

        /// <summary>
        /// Returns a SQL expression that checks if a GIS coordinate is within a polygon using PostGIS UDF call.
        /// </summary>
        /// <param name="coordinate"></param>
        /// <param name="polygon"></param>
        /// <returns></returns>
        // public static string WithinExpr(PostGisCoordinate coordinate, List<PostGisCoordinate> polygon)
        // {
        //     var c = CultureInfo.GetCultureInfo("en-US");
        //     // Create PostGIS points out of a list of polygon coordinates where coordinates are pairs of lat/long values
        //     var points = new List<string>();
        //     for (int i = 0; i < polygon.Count; i++)
        //         points.Add($"ST_MakePoint({polygon[i].Latitude.ToString(c)}, {polygon[i].Longitude.ToString(c)})");
        //     // Create a PostGIS polygon out of the points
        //     var polygonExpr = $"ST_MakePolygon(ST_MakeLine(ARRAY[{String.Join(", ", points)}]))";
        //     // Create a PostGIS point out of the target coordinate
        //     var pointExpr = $"ST_MakePoint({coordinate})";
        //     // Return the PostGIS UDF call
        //     return $"ST_Within({pointExpr}, {polygonExpr})";
        // }

        public static string WithinExpr(List<decimal> coordinate, List<decimal> polygon)
        {
            var c = CultureInfo.GetCultureInfo("en-US");
            if (coordinate.Count != 2 || polygon.Count % 2 != 0 || polygon.Count < 6)
                throw new ArgumentException("Invalid coordinate or polygon sizes");
            // Create PostGIS points out of a list of polygon coordinates where coordinates are pairs of lat/long values
            var points = new List<string>();
            for (int i = 0; i < polygon.Count; i += 2)
                points.Add($"ST_MakePoint({polygon[i].ToString(c)}, {polygon[i+1].ToString(c)})");
            // Create a PostGIS polygon out of the points
            var polygonExpr = $"ST_MakePolygon(ST_MakeLine(ARRAY[{String.Join(", ", points)}]))";
            // Create a PostGIS point out of the target coordinate
            var pointExpr = $"ST_MakePoint({coordinate})";
            // Return the PostGIS UDF call
            return $"ST_Within({pointExpr}, {polygonExpr})";
        }
    }

    // // Create a class that represents a PostGIS coordinate
    // public class PostGisCoordinate(decimal latitude, decimal longitude)
    // {
    //     public decimal Latitude { get; set; } = latitude;
    //     public decimal Longitude { get; set; } = longitude;

    //     public string ToString(CultureInfo cultureInfo)
    //     {
    //         return $"{Latitude.ToString(cultureInfo)}, {Longitude.ToString(cultureInfo)}";
    //     }

    //     public override string ToString()
    //     {
    //         return ToString(CultureInfo.GetCultureInfo("en-US"));
    //     }
    // }
}