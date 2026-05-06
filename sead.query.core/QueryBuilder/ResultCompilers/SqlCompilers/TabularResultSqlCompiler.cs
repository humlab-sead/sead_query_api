using SeadQueryCore.Model.Ext;
using System.Collections.Generic;
using System.Linq;

namespace SeadQueryCore
{
    public class TabularResultSqlCompiler : IResultSqlCompiler
    {
        private const string CountryResultFieldKey = "country";

        string IResultSqlCompiler.ViewTypeId => "tabular";
        
        public string Compile(QueryBuilder.QuerySetup querySetup, Facet notUsed, IEnumerable<ResultSpecificationField> fields)
        {
            var resultFields = fields.ToList();
            string sql = $@"
            SELECT {resultFields.GetResultCompiledValueFields().ToList().Combine(", ")}
            FROM (
                SELECT {resultFields.GetResultColumnNameAliasPairs().Select(x => $"{x.ColumnName} AS {x.Alias}").ToList().Combine(", ")}
                FROM {querySetup.Facet.TargetTable.ResolvedSqlJoinName}
                     {querySetup.Joins.Combine("")}
                     {GetExtraJoinSql(resultFields)}
                WHERE 1 = 1
                {"AND ".GlueTo(querySetup.Criterias.Combine(" AND "))}
                GROUP BY {resultFields.GetResultInnerGroupByFields().ToList().Combine(", ")}
            ) AS X
            {"GROUP BY ".GlueTo(resultFields.GetResultGroupByFields().ToList().Combine(", "))}
            {"ORDER BY ".GlueTo(resultFields.GetResultSortFields().ToList().Combine(", "))}
        ";
            return sql;
        }

        private static string GetExtraJoinSql(List<ResultSpecificationField> fields)
        {
            if (!fields.Any(z => z.ResultField?.ResultFieldKey == CountryResultFieldKey))
                return "";

            return @"
                LEFT JOIN (
                    SELECT site_country_links.site_id,
                           STRING_AGG(DISTINCT country_locations.location_name, '  ') AS country_name
                    FROM tbl_site_locations site_country_links
                    INNER JOIN tbl_locations country_locations
                        ON country_locations.location_id = site_country_links.location_id
                       AND country_locations.location_type_id = 1
                    GROUP BY site_country_links.site_id
                ) site_country ON site_country.site_id = tbl_sites.site_id
            ";
        }
    }
}
