using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

using static QueryFacetDomain.Utility;

namespace QueryFacetDomain {

    class SqlFieldCompiler {
        protected static Dictionary<string, SqlFieldCompiler> compilers = null;

        public static SqlFieldCompiler getCompiler(string type)
        {
            return isFieldType(type) ? getCompilers()[type] : null;
        }

        public static bool isFieldType(string type)
        {
            return getCompilers().ContainsKey(type);
        }

        public static bool isSingleItemType(string type)
        {
            return isFieldType(type) && (getCompiler(type).GetType().Name == "SqlFieldCompiler");
        }

        public static bool isAggregateType(string type)
        {
            return isFieldType(type) && !isSingleItemType(type);
        }

        public static bool isSortType(string type)
        {
            return type == "sort_item";
        }

        public static bool isGroupByType(string type)
        {
            return isSingleItemType(type) || isSortType(type);
        }

        public static Dictionary<string, SqlFieldCompiler> getCompilers()
        {
            if (compilers == null)
                compilers = new Dictionary<string, SqlFieldCompiler>() {
                { "sum_item", new SumFieldCompiler() },
                { "count_item", new CountFieldCompiler() },
                { "avg_item", new AvgFieldCompiler() },
                { "text_agg_item", new TextAggFieldCompiler() },
                { "single_item", new SqlFieldCompiler() },
                { "link_item", new SqlFieldCompiler() },
                { "link_item_filtered", new SqlFieldCompiler() }
             };
            return compilers;
        }

        public virtual string compile(string key) { return key; }
    }

    class SumFieldCompiler : SqlFieldCompiler {
        public override string compile(string key) { return $"SUM({key}.double precision) AS sum_of_{key}"; }
    }

    class CountFieldCompiler : SqlFieldCompiler {
        public override string compile(string key) { return $"COUNT({key}) AS count_of_{key}"; }
    }

    class AvgFieldCompiler : SqlFieldCompiler {
        public override string compile(string key) { return $"AVG({key}) AS avg_of_{key}"; }
    }

    class TextAggFieldCompiler : SqlFieldCompiler {
        public override string compile(string key) { return $"array_to_string(array_agg(DISTINCT {key}),',') AS text_agg_of_{key}"; }
    }

    class SqlQueryBuilder {
    }

    class ValidPicksSqlQueryBuilder {
        //public static function deleteBogusPicks(&facetsConfig)
        public static string compile(QueryBuilder.QuerySetup query, FacetDefinition facet, List<int> picks)
        {
            string picks_clause = array_join_surround(picks, ",", "('", "'::text)", "");
            string sql = $@"
            SELECT DISTINCT pick_id, {facet.CategoryNameExpr} AS name
            FROM {query.sql_table}
            JOIN (VALUES {picks_clause}) AS x(pick_id)
              ON x.pick_id = {facet.CategoryIdExpr}::text
              {query.sql_joins}
            WHERE 1 = 1
              {query.sql_where2}
        ";
            return sql;
        }
    }

    class RangeCounterSqlQueryBuilder {
        
        public static string compile(QueryBuilder.QuerySetup query, FacetDefinition facet, string intervalQuery, string countColumn)
        {
            string sql = $@"
            SELECT category, count(category) AS count, lower, upper
            FROM (
                SELECT COALESCE(lower||' => '||upper, 'data missing') AS category, group_column, lower, upper
                FROM  (
                    SELECT lower, upper, {countColumn} AS group_column
                    FROM {query.sql_table}
                    LEFT JOIN ( {intervalQuery} ) AS temp_interval
                        ON {facet.CategoryIdExpr}::integer >= lower
                        AND {facet.CategoryIdExpr}::integer < upper
                            {query.sql_joins}
                    {query.sql_where2}
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
        
        public static string compile(QueryBuilder.QuerySetup query, FacetDefinition facet, FacetDefinition countFacet, string aggType)
        {
            string sql = $@"
            SELECT category, {aggType}(value) AS count
            FROM (
                SELECT {facet.CategoryIdExpr} AS category, {countFacet.CategoryIdExpr} AS count
                FROM {query.sql_table}
                     {query.sql_joins}
                WHERE 1 = 1
                    {query.sql_where2}
                GROUP BY {facet.CategoryIdExpr}, {countFacet.CategoryIdExpr}
            ) AS x
            GROUP BY category;
        ";
            return sql;
        }
    }

    class RangeCategoryBoundSqlQueryBuilder {
        // RangeMinMaxFacetCounter::templateSQL(query, facet, facetCode): string
        public string compile(QueryBuilder.QuerySetup query, FacetDefinition facet, string facetCode)
        {
            string clauses = String.Join("", facet.Clauses.Select(x => x.Clause));
            string where_clause = str_prefix("WHERE ", clauses);
            string sql = $@"
             SELECT 'facetCode' AS facet_code, MIN({facet.CategoryIdExpr}::real) AS min, MAX({facet.CategoryIdExpr}::real) AS max
             FROM {facet.TargetTableName} 
               {query.sql_joins} 
             {where_clause}";
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
                FROM {query.sql_table} 
                     {query.sql_joins}
                WHERE 1 = 1
                  {query.sql_where2}
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
        public static string compile(int interval, int min, int max, int interval_count)
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
        public static string compile(QueryBuilder.QuerySetup query, FacetDefinition facet, string text_filter)
        {
            string text_criteria = empty(text_filter) ? ""
                : $" AND {facet.CategoryNameExpr} ILIKE '{text_filter}' ";
            string sort_clause = empty(facet.SortExpr) ? ""
                : $", {facet.SortExpr} ORDER BY {facet.SortExpr}";
            string sql = $@"
            SELECT {facet.CategoryIdExpr} AS category, {facet.CategoryNameExpr} AS name
            FROM {query.sql_table}
                 {query.sql_joins}
            WHERE 1 = 1
              {text_criteria}
              {query.sql_where2}
            GROUP BY {facet.CategoryIdExpr}, {facet.CategoryNameExpr}
            {sort_clause}";
            return sql;
        }

    }

    public class ResultSqlQueryBuilder {
        // ResultSqlQueryCompiler::compile(query, queryConfig): string
        public static string compile(QueryBuilder.QuerySetup query, FacetDefinition facet, QueryConfig queryConfig)
        {
            string data_fields = String.Join(", ", queryConfig.data_fields);
            string group_by_inner_fields = String.Join(", ", queryConfig.group_by_inner_fields);
            string data_fields_alias = String.Join(", ", queryConfig.data_fields_alias);
            string group_by_clause = str_prefix("GROUP BY ", String.Join(", ", queryConfig.group_by_fields));
            string sort_by_clause = str_prefix("ORDER BY ", String.Join(", ", queryConfig.sort_fields));

            string sql = $@"
            SELECT {data_fields}
            FROM (
                SELECT {data_fields_alias}
                FROM {query.sql_table}
                    {query.sql_joins}
                WHERE 1 = 1 {query.sql_where2}
                GROUP BY {group_by_inner_fields}
            ) AS X 
            {group_by_clause}
            {sort_by_clause}
        ";
            return sql;
        }
    }

    class MapResultSqlQueryBuilder {
        // MapResultSqlQueryCompiler::compileSQL(query, facet): string
        public static string compile(QueryBuilder.QuerySetup query, FacetDefinition facet)
        {
            string sql = $@"
            SELECT DISTINCT {facet.CategoryNameExpr} AS name, latitude_dd, longitude_dd, {facet.CategoryIdExpr} AS id_column
            FROM {query.sql_table}
                {query.sql_joins}
            WHERE 1 = 1 {query.sql_where2}
        ";
            return sql;
        }
    }
}