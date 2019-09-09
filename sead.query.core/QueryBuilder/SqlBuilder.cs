//using Newtonsoft.Json;
using SeadQueryCore.QueryBuilder;
using System;
using System.Collections.Generic;
using System.Linq;
using static SeadQueryCore.Utility;

namespace SeadQueryCore
{
    #region __SQL field compilers__

    public interface ISqlFieldCompiler
    {
        string Compile(string expr);
    }

    public class SqlFieldCompiler : ISqlFieldCompiler
    {
        /* [JsonIgnore] */ public ResultFieldType FieldType { get; private set; }
        public SqlFieldCompiler(ResultFieldType fieldType)
        {
            FieldType = fieldType;
        }
        public virtual string Compile(string expr) { return expr; }
    }

    public class DefaultFieldCompiler : SqlFieldCompiler
    {
        public DefaultFieldCompiler(ResultFieldType fieldType) : base(fieldType) { }
        public override string Compile(string expr) { return expr; }
    }

    public class SumFieldCompiler : SqlFieldCompiler
    {
        public SumFieldCompiler(ResultFieldType fieldType) : base(fieldType) { }
        public override string Compile(string expr) { return $"SUM({expr}.double precision) AS sum_of_{expr}"; }
    }

    public class CountFieldCompiler : SqlFieldCompiler
    {
        public CountFieldCompiler(ResultFieldType fieldType) : base(fieldType) { }
        public override string Compile(string expr) { return $"COUNT({expr}) AS count_of_{expr}"; }
    }

    public class AvgFieldCompiler : SqlFieldCompiler
    {
        public AvgFieldCompiler(ResultFieldType fieldType) : base(fieldType) { }
        public override string Compile(string expr) { return $"AVG({expr}) AS avg_of_{expr}"; }
    }

    public class TextAggFieldCompiler : SqlFieldCompiler {
        public TextAggFieldCompiler(ResultFieldType fieldType) : base(fieldType) { }
        public override string Compile(string expr) { return $"array_to_string(array_agg(DISTINCT {expr}),',') AS text_agg_of_{expr}"; }
    }

    public class TemplateFieldCompiler : SqlFieldCompiler
    {
        public TemplateFieldCompiler(ResultFieldType fieldType) : base(fieldType) { }
        public override string Compile(string expr) { return string.Format(FieldType.SqlTemplate, expr); }
    }
    #endregion

    public static class ValidPicksSqlQueryBuilder {
        public static string Compile(QueryBuilder.QuerySetup query, Facet facet, List<int> picks)
        {
            string picks_clause = picks.Combine(",", x => $"('{x}'::text)");
            string sql = $@"
            SELECT DISTINCT pick_id, {facet.CategoryNameExpr} AS name
            FROM {query.Facet.TargetTableName} {"AS ".GlueTo(query.Facet.AliasName)}
            JOIN (VALUES {picks_clause}) AS x(pick_id)
              ON x.pick_id = {facet.CategoryIdExpr}::text
              {query.Joins.Combine("")}
            WHERE 1 = 1
              {" AND ".GlueTo(query.Criterias.Combine(" AND "))}
        ";
            return sql;
        }
    }

    public static class RangeCounterSqlQueryBuilder {
        public static string Compile(QueryBuilder.QuerySetup query, Facet facet, string intervalQuery, string countColumn)
        {
            string sql = $@"
            WITH categories(category, lower, upper) AS ({intervalQuery})
                SELECT category, lower, upper, COUNT(DISTINCT {countColumn}) AS count_column
                FROM categories
                LEFT JOIN {query.Facet.TargetTableName} {"AS ".GlueTo(query.Facet.AliasName)}
                  ON {facet.CategoryIdExpr}::integer >= lower
                 AND {facet.CategoryIdExpr}::integer <= upper
                {query.Joins.Combine("")}
                {"AND ".GlueTo(query.Criterias.Combine(" AND "))}
                GROUP BY category, lower, upper
                ORDER BY lower";
            return sql;
            //string sql = $@"
            //SELECT category, count(category) AS count, lower, upper
            //FROM (
            //    SELECT COALESCE(lower || ' => ' || upper, 'data missing') AS category, count_column, lower, upper
            //    FROM  (
            //        SELECT lower, upper, {countColumn} AS count_column
            //        FROM {query.Facet.TargetTableName} {"AS ".AddIf(query.Facet.AliasName)}
            //        LEFT JOIN ( {intervalQuery} ) AS temp_interval
            //            ON {facet.CategoryIdExpr}::integer >= lower
            //            AND {facet.CategoryIdExpr}::integer < upper
            //                {query.Joins.Combine("")}
            //          {"AND ".AddIf(query.Criterias.Combine(" AND "))}
            //        GROUP BY lower, upper, {countColumn}
            //        ORDER BY lower) AS x
            //    GROUP by lower, upper, count_column) AS y
            //WHERE lower is not null
            //AND upper is not null
            //GROUP BY category, lower, upper
            //ORDER BY lower, upper";
            //return sql;
        }
    }

    public static class DiscreteCounterSqlQueryBuilder {
        public static string Compile(QueryBuilder.QuerySetup query, Facet facet, Facet countFacet, string aggType)
        {
            string sql = $@"
            SELECT category, {aggType}(value) AS count
            FROM (
                SELECT {facet.CategoryIdExpr} AS category, {countFacet.CategoryIdExpr} AS value
                FROM {query.Facet.TargetTableName} {"AS ".GlueTo(query.Facet.AliasName)}
                     {query.Joins.Combine("")}
                WHERE 1 = 1
                {"AND ".GlueTo(query.Criterias.Combine(" AND "))}
                GROUP BY {facet.CategoryIdExpr}, {countFacet.CategoryIdExpr}
            ) AS x
            GROUP BY category;
        ";
            return sql;
        }
    }

    public interface ICategoryBoundSqlQueryBuilder
    {
        string Compile(QuerySetup query, Facet facet, string facetCode);
    }

    public class RangeCategoryBoundSqlQueryBuilder : ICategoryBoundSqlQueryBuilder
    {
        public string Compile(QuerySetup query, Facet facet, string facetCode)
        {
            string clauses = String.Join("", facet.Clauses.Select(x => x.Clause));
            string sql = $@"
               SELECT '{facetCode}' AS facet_code, MIN({facet.CategoryIdExpr}::real) AS min, MAX({facet.CategoryIdExpr}::real) AS max
               FROM {facet.TargetTableName}
                 {query.Joins.Combine("")}
             {"WHERE ".GlueTo(clauses)}";
            return sql;
        }
    }

    public static class FacetContentExtraRowInfoSqlQueryBuilder {
        public static string Compile(QueryBuilder.QuerySetup query, Facet facet)
        {
            string sql = $@"
            SELECT DISTINCT id, name
            FROM (
                SELECT {facet.CategoryIdExpr} AS id, COALESCE({facet.CategoryNameExpr},'No value') AS name, {facet.SortExpr} AS sort_column
                FROM {query.Facet.TargetTableName} {"AS ".GlueTo(query.Facet.AliasName)}
                     {query.Joins.Combine("")}
                WHERE 1 = 1
                {"AND ".GlueTo(query.Criterias.Combine(" AND "))}
                GROUP BY name, id, sort_column
                ORDER BY {facet.SortExpr}
            ) AS tmp
        ";
            return sql;
        }
    }

    public static class RangeLowerUpperSqlQueryBuilder {
        public static string compile(QueryBuilder.QuerySetup query, Facet facet)
        {
            string sql = $@"
          SELECT MIN({facet.CategoryIdExpr}) AS lower, MAX({facet.CategoryIdExpr}) AS upper
          FROM {facet.TargetTableName}
        ";
            return sql;
        }
    }

    public static class RangeIntervalSqlQueryBuilder {
        public static string Compile(int interval, int min, int max, int interval_count)
        {
            List<string> pieces = new List<string>();
            //for (int i = 0, lower = min; i <= interval_count && lower <= max; i++) {
            //    int upper = lower + interval;
            //    pieces.Add($@"('{lower} => {upper}', {lower}, {upper})");
            //    lower += interval;
            //}
            //int lower = min;
            //while (lower <= max) {
            //    int upper = lower + interval;
            //    pieces.Add($@"('{lower} => {upper}', {lower}, {upper})");
            //    lower = upper;
            //}
            //string values = String.Join("\n,", pieces);
            //string sql = $"(VALUES {values})";

            string sql = $"" +
                $"(SELECT n::text || ' => ' || (n + {interval})::text, n, n + {interval} \n" +
                $" FROM generate_series({min}, {max}, {interval}) as a(n) WHERE n < {max})\n";
            return sql;
        }
    }

    public static class DiscreteContentSqlQueryBuilder {
        public static string Compile(QueryBuilder.QuerySetup query, Facet facet, string text_filter)
        {
            string text_criteria = text_filter.IsEmpty() ? "" : $" AND {facet.CategoryNameExpr} ILIKE '{text_filter}' ";
            string sort_clause = empty(facet.SortExpr) ? "" : $", {facet.SortExpr} ORDER BY {facet.SortExpr}";

            string sql = $@"
            SELECT cast({facet.CategoryIdExpr} AS varchar) AS category, {facet.CategoryNameExpr} AS name
            FROM {query.Facet.TargetTableName} {"AS ".GlueTo(query.Facet.AliasName)}
                 {query.Joins.Combine("")}
            WHERE 1 = 1
              {text_criteria}
            {"AND ".GlueTo(query.Criterias.Combine(" AND "))}
            GROUP BY {facet.CategoryIdExpr}, {facet.CategoryNameExpr}
            {sort_clause}";
            return sql;
        }
    }

    #region IResultSqlQueryCompiler

    public interface IResultSqlQueryCompiler
    {
        string Compile(QueryBuilder.QuerySetup query, Facet facet, ResultQuerySetup config);
    }

    public class TabularResultSqlQueryBuilder : IResultSqlQueryCompiler {
        public string Compile(QueryBuilder.QuerySetup query, Facet facet, ResultQuerySetup config)
        {
            string sql = $@"
            SELECT {config.DataFields.Combine(", ")}
            FROM (
                SELECT {config.AliasPairs.Select(x => $"{x.Item1} AS {x.Item2}").ToList().Combine(", ")}
                FROM {query.Facet.TargetTableName} {"AS ".GlueTo(query.Facet.AliasName)}
                     {query.Joins.Combine("")}
                WHERE 1 = 1
                {"AND ".GlueTo(query.Criterias.Combine(" AND "))}
                GROUP BY {config.InnerGroupByFields.Combine(", ")}
            ) AS X
            {"GROUP BY ".GlueTo(config.GroupByFields.Combine(", "))}
            {"ORDER BY ".GlueTo(config.SortFields.Combine(", "))}
        ";
            return sql;
        }
    }

    public class MapResultSqlQueryBuilder : IResultSqlQueryCompiler {
        // FIXME Use ResultQuerySetup to build query. If possible, merge TabularResultSqlQueryBuilder & MapResultSqlQueryBuilder
        public string Compile(QueryBuilder.QuerySetup query, Facet facet, ResultQuerySetup config)
        {
            string sql = $@"
            SELECT DISTINCT {facet.CategoryIdExpr} AS id_column, {facet.CategoryNameExpr} AS name, coalesce(latitude_dd, 0.0) AS latitude_dd, coalesce(longitude_dd, 0) AS longitude_dd
            FROM {query.Facet.TargetTableName} {"AS ".GlueTo(query.Facet.AliasName)}
                 {query.Joins.Combine("")}
            WHERE 1 = 1
            {"AND ".GlueTo(query.Criterias.Combine(" AND "))}
        ";
            return sql;
        }
    }
    #endregion

    public static class JoinClauseCompiler
    {
        public static string Compile(IFacetsGraph graph, GraphEdge edge, bool innerJoin = false)
        {
            var resolvedTableName = graph.ResolveTargetName(edge.TargetTableName);
            var resolvedAliasName = graph.ResolveAliasName(edge.TargetTableName);
            var joinType = innerJoin ? "INNER" : "LEFT";
            var sql = $" {joinType} JOIN {resolvedTableName} {resolvedAliasName ?? ""}" +
                    $" ON {resolvedAliasName ?? resolvedTableName}.\"{edge.TargetColumnName}\" = " +
                            $"{edge.SourceTableName}.\"{edge.SourceColumnName}\" ";
            return sql;
        }
    }

    public static class UtilitySqlCompiler
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