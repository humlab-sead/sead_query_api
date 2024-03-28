using SeadQueryCore;
using SeadQueryCore.QueryBuilder;
using System.Collections.Generic;
using System.Xml.Serialization;
using Xunit;

namespace SQT.SqlCompilers
{
    public class SqlCompileUtilityTests
    {

        [Fact]
        public void SqlCompileInExprTests()
        {
            SqlCompileUtility.InExpr("A", new List<int>() { 1, 2, 3 }).Equals("A in (1, 2, 3)");
            SqlCompileUtility.InExpr("A", new List<int>() { 1 }).Equals("A in (1)");
            SqlCompileUtility.InExpr("A", new List<int>() { }).Equals("A in ()");

            SqlCompileUtility.InExpr("A", new List<string>() { "1", "2", "3" }).Equals("A in ('1', '2', '3')");
            SqlCompileUtility.InExpr("A", new List<string>() { "1" }).Equals("A in ('1')");
            SqlCompileUtility.InExpr("A", new List<string>() { }).Equals("A in ()");

            SqlCompileUtility.InExpr("A", new List<decimal>() { 1.1M, 2.2M, 3.3M }).Equals("A in (1.1, 2.2, 3.3)");
            SqlCompileUtility.InExpr("A", new List<decimal>() { 1.1M }).Equals("A in (1.1)");
            SqlCompileUtility.InExpr("A", new List<decimal>() { }).Equals("A in ()");
            
        }

        [Fact]
        public void SqlCompileBetweenExprTests()
        {
            SqlCompileUtility.BetweenExpr("A", 1, 2).Equals("A between 1 and 2");
            SqlCompileUtility.BetweenExpr("A", 1.1M, 2.2M).Equals("A between 1.1 and 2.2");

            SqlCompileUtility.BetweenExpr("A", 1, 1).Equals("A between 1 and 1");
            SqlCompileUtility.BetweenExpr("A", 1.1M, 1.1M).Equals("A between 1.1 and 1.1");

        }

        [Fact]
        public void SqlCompileLikeExprTests()
        {
            SqlCompileUtility.LikeExpr("A", "B").Equals("A like 'B'");
            SqlCompileUtility.LikeExpr("A", "B%").Equals("A like 'B%'");
            SqlCompileUtility.LikeExpr("A", "%B").Equals("A like '%B%'");
            SqlCompileUtility.LikeExpr("A", "%B%").Equals("A like '%B%'");
        }

        // [Fact]
        // public void SqlCompileWithinExprTests()
        // {
        //     PostGisCoordinate coordinate = new PostGisCoordinate(1.1M, 2.2M);
        //     List<PostGisCoordinate> polygon = new List<PostGisCoordinate>() {
        //         new PostGisCoordinate(1.1M, 2.2M),
        //         new PostGisCoordinate(3.3M, 4.4M),
        //         new PostGisCoordinate(5.5M, 6.6M)
        //     };
        //     SqlCompileUtility.WithinExpr(coordinate, polygon).Equals("ST_Within(ST_MakePoint(1.1, 2.2), ST_MakePolygon(ST_MakeLine(ARRAY[ST_MakePoint(1.1, 2.2), ST_MakePoint(3.3, 4.4), ST_MakePoint(5.5, 6.6)]))");
        // }

        [Fact]
        public void SqlCompileWithinExprTests2()
        {            
            List<decimal> coordinate = [1.1M, 2.2M];
            List<decimal> polygon = [
                1.1M, 2.2M,
                3.3M, 4.4M,
                5.5M, 6.6M
            ];
            SqlCompileUtility.WithinExpr(coordinate, polygon).Equals("ST_Within(ST_MakePoint(1.1, 2.2), ST_MakePolygon(ST_MakeLine(ARRAY[ST_MakePoint(1.1, 2.2), ST_MakePoint(3.3, 4.4), ST_MakePoint(5.5, 6.6)]))");
        }

    }
}
