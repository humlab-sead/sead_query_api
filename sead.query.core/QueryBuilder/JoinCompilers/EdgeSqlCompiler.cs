
using System.Collections.Generic;

namespace SeadQueryCore
{
    public class EdgeSqlCompiler : IEdgeSqlCompiler
    {

        public Dictionary<bool, string> Join = new Dictionary<bool, string> {
            { true, "INNER" },
            { false, "LEFT" }
        };


        public string Compile(TableRelation edge, FacetTable targetTable, bool innerJoin = false)
        {

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