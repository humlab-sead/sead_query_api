
using System.Collections.Generic;

namespace SeadQueryCore
{
    public class EdgeSqlCompiler : IEdgeSqlCompiler
    {

        public Dictionary<bool, string> Join = new Dictionary<bool, string> {
            { true, "INNER" },
            { false, "LEFT" }
        };


        public string Compile(TableRelation edge, FacetTable targetTable, Dictionary<string, FacetTable> aliases, bool innerJoin = false)
        {
            /* 
             * A table alias is _unique_ to a specific FacetTable i.e. the sam alias cannot occur in more than one facet
             */

            if (targetTable == null) {
                /* Fetch alias FacetTable if exists */
                if (aliases.ContainsKey(edge.TargetName)) {
                    targetTable = aliases[edge.TargetName];
                }
            }

            /* 
             * If "facetTable" exists, then the target table is found in FacetsConfig,
             * otherwise it is a table found by the route finding service.
             */

            var sql = $" {Join[innerJoin]} JOIN {targetTable?.ResolvedSqlJoinName ?? edge.TargetName} " +
                        $"ON {targetTable?.ResolvedAliasOrTableOrUdfName ?? edge.TargetName}.\"{edge.TargetColumnName}\" = " +
                                $"{edge.SourceName}.\"{edge.SourceColumName}\" ";

            return sql;
        }
    }
}