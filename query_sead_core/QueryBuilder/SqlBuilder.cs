using Newtonsoft.Json;
using QuerySeadDomain.QueryBuilder;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using static QuerySeadDomain.Utility;

namespace QuerySeadDomain {

    public interface ISqlFieldCompiler
    {
        string Compile(string expr);
    }
    
    public class SqlFieldCompiler : ISqlFieldCompiler
    {
        [JsonIgnore] public ResultFieldType FieldType { get; private set; }
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

    class ValidPicksSqlQueryBuilder {
        //public static function deleteBogusPicks(&facetsConfig)
        public static string Compile(QueryBuilder.QuerySetup query, FacetDefinition facet, List<int> picks)
        {
            string picks_clause = picks.Combine(",", x => $"('{x}'::text)");
            string sql = $@"
            SELECT DISTINCT pick_id, {facet.CategoryNameExpr} AS name
            FROM {query.Facet.TargetTableName} {"AS ".AddIf(query.Facet.AliasName)}
            JOIN (VALUES {picks_clause}) AS x(pick_id)
              ON x.pick_id = {facet.CategoryIdExpr}::text
              {query.Joins.Combine("")}
            WHERE 1 = 1
              {"AND ".AddIf(query.Criterias.Combine(" AND "))}
        ";
            return sql;
        }
    }

    class RangeCounterSqlQueryBuilder {
        
        public static string Compile(QueryBuilder.QuerySetup query, FacetDefinition facet, string intervalQuery, string countColumn)
        {
            string sql = $@"
            SELECT category, count(category) AS count, lower, upper
            FROM (
                SELECT COALESCE(lower||' => '||upper, 'data missing') AS category, group_column, lower, upper
                FROM  (
                    SELECT lower, upper, {countColumn} AS group_column
                    FROM {query.Facet.TargetTableName} {"AS ".AddIf(query.Facet.AliasName)}
                    LEFT JOIN ( {intervalQuery} ) AS temp_interval
                        ON {facet.CategoryIdExpr}::integer >= lower
                        AND {facet.CategoryIdExpr}::integer < upper
                            {query.Joins.Combine("")}
                      {"AND ".AddIf(query.Criterias.Combine(" AND "))}
                    GROUP BY lower, upper, {countColumn}
                    ORDER BY lower) AS x
                GROUP by lower, upper, group_column) AS y
            WHERE lower is not null
            AND upper is not null
            GROUP BY category, lower, upper
            ORDER BY lower, upper";
            return sql;
        }
    }

    class DiscreteCounterSqlQueryBuilder {
        
        public static string Compile(QueryBuilder.QuerySetup query, FacetDefinition facet, FacetDefinition countFacet, string aggType)
        {
            string sql = $@"
            SELECT category, {aggType}(value) AS count
            FROM (
                SELECT {facet.CategoryIdExpr} AS category, {countFacet.CategoryIdExpr} AS value
                FROM {query.Facet.TargetTableName} {"AS ".AddIf(query.Facet.AliasName)}
                     {query.Joins.Combine("")}
                WHERE 1 = 1
                {"AND ".AddIf(query.Criterias.Combine(" AND "))}
                GROUP BY {facet.CategoryIdExpr}, {countFacet.CategoryIdExpr}
            ) AS x
            GROUP BY category;
        ";
            return sql;
        }
    }

    class RangeCategoryBoundSqlQueryBuilder {
        // RangeMinMaxFacetCounter::templateSQL(query, facet, facetCode): string
        public string Compile(QueryBuilder.QuerySetup query, FacetDefinition facet, string facetCode)
        {
            string clauses = String.Join("", facet.Clauses.Select(x => x.Clause));
            string sql = $@"
               SELECT 'facetCode' AS facet_code, MIN({facet.CategoryIdExpr}::real) AS min, MAX({facet.CategoryIdExpr}::real) AS max
               FROM {facet.TargetTableName} 
                 {query.Joins.Combine("")} 
             {"WHERE ".AddIf(clauses)}";
            return sql;
        }
    }

    class FacetContentExtraRowInfoSqlQueryBuilder {
        // FacetContentLoader::getExtraRowInfo
        public static string compile(QueryBuilder.QuerySetup query, FacetDefinition facet)
        {
            string sql = $@"
            SELECT DISTINCT id, name
            FROM (
                SELECT {facet.CategoryIdExpr} AS id, COALESCE({facet.CategoryNameExpr},'No value') AS name, {facet.SortExpr} AS sort_column
                FROM {query.Facet.TargetTableName} {"AS ".AddIf(query.Facet.AliasName)}
                     {query.Joins.Combine("")}
                WHERE 1 = 1
                {"AND ".AddIf(query.Criterias.Combine(" AND "))}
                GROUP BY name, id, sort_column
                ORDER BY {facet.SortExpr}
            ) AS tmp
        ";
            return sql;
        }
    }

    class RangeLowerUpperSqlQueryBuilder {
        // RangeFacetContentLoader::computeRangeLowerUpper(facetCode)
        public static string compile(QueryBuilder.QuerySetup query, FacetDefinition facet)
        {
            string sql = $@"
          SELECT MIN({facet.CategoryIdExpr}) AS lower, MAX({facet.CategoryIdExpr}) AS upper
          FROM {facet.TargetTableName}
        ";
            return sql;
        }
    }

    class RangeIntervalSqlQueryBuilder {
        // RangeFacetContentLoader::getRangeQuery($interval, $min_value, $max_value, $interval_count)
        public static string Compile(int interval, int min, int max, int interval_count)
        {
            List<string> pieces = new List<string>();
            for (int i = 0, lower = min; i <= interval_count && lower <= max; i++) {
                int upper = lower + interval;
                pieces.Add($@"('{lower} => {upper}', {lower}, {upper})");
                lower += interval;
            }
            string values = String.Join("\n,", pieces);
            string sql = $@"
                SELECT category, lower, upper, category as name
                FROM (VALUES {values}) AS X(category, lower, upper)
            ";
            return sql;
        }
    }

    class DiscreteContentSqlQueryBuilder {
        // DiscreteFacetContentLoader::compileSQL(facetsConfig, facet, query): string
        public static string Compile(QueryBuilder.QuerySetup query, FacetDefinition facet, string text_filter)
        {
            string text_criteria = text_filter.IsEmpty() ? "" : $" AND {facet.CategoryNameExpr} ILIKE '{text_filter}' ";
            string sort_clause = empty(facet.SortExpr) ? "" : $", {facet.SortExpr} ORDER BY {facet.SortExpr}";

            string sql = $@"
            SELECT {facet.CategoryIdExpr} AS category, {facet.CategoryNameExpr} AS name
            FROM {query.Facet.TargetTableName} {"AS ".AddIf(query.Facet.AliasName)}
                 {query.Joins.Combine("")}
            WHERE 1 = 1
              {text_criteria}
            {"AND ".AddIf(query.Criterias.Combine(" AND "))}
            GROUP BY {facet.CategoryIdExpr}, {facet.CategoryNameExpr}
            {sort_clause}";
            return sql;
        }

    }
    public interface IResultSqlQueryCompiler
    {
        string Compile(QueryBuilder.QuerySetup query, FacetDefinition facet, ResultQuerySetup config);
    }

    public class TabularResultSqlQueryBuilder : IResultSqlQueryCompiler {

        public string Compile(QueryBuilder.QuerySetup query, FacetDefinition facet, ResultQuerySetup config)
        {
            string sql = $@"
            SELECT {config.DataFields.Combine(", ")}
            FROM (
                SELECT {config.AliasPairs.Select(x => $"{x.Item1} AS {x.Item2}").ToList().Combine(", ")}
                FROM {query.Facet.TargetTableName} {"AS ".AddIf(query.Facet.AliasName)}
                     {query.Joins.Combine("")}
                WHERE 1 = 1
                {"AND ".AddIf(query.Criterias.Combine(" AND "))}
                GROUP BY {config.InnerGroupByFields.Combine(", ")}
            ) AS X 
            {"GROUP BY ".AddIf(config.GroupByFields.Combine(", "))}
            {"ORDER BY ".AddIf(config.SortFields.Combine(", "))}
        ";
            return sql;
        }
    }

    public class MapResultSqlQueryBuilder : IResultSqlQueryCompiler {

        public string Compile(QueryBuilder.QuerySetup query, FacetDefinition facet, ResultQuerySetup config)
        {
            string sql = $@"
            SELECT DISTINCT {facet.CategoryNameExpr} AS name, latitude_dd, longitude_dd, {facet.CategoryIdExpr} AS id_column
            FROM {query.Facet.TargetTableName} {"AS ".AddIf(query.Facet.AliasName)}
                 {query.Joins.Combine("")}
            WHERE 1 = 1
            {"AND ".AddIf(query.Criterias.Combine(" AND "))}
        ";
            return sql;
        }
    }

    public class JoinClauseCompiler
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
}