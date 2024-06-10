﻿using System.Collections.Generic;
using System.Linq;

namespace SeadQueryCore.QueryBuilder
{
    public class JoinsClauseCompiler(IRouteFinder graph, IJoinSqlCompiler joinCompiler) : IJoinsClauseCompiler
    {
        public IJoinSqlCompiler JoinCompiler { get; } = joinCompiler;
        public IRouteFinder FacetsGraph { get; set; } = graph;

        private FacetTable GetFacetTableByNameOrAlias(FacetsConfig2 facetsConfig, TableRelation edge)
            => facetsConfig.GetFacetTable(edge.TargetName) ?? FacetsGraph.GetAliasedFacetTable(edge.TargetName);

        public virtual List<string> Compile(FacetsConfig2 facetsConfig, Facet targetFacet, List<string> involvedTables)
        {
            var routes = FacetsGraph.Find(targetFacet.TargetTable.ResolvedAliasOrTableOrUdfName, involvedTables, true);
            var joins = routes.Edges().Select(
                    edge => JoinCompiler.Compile(edge, GetFacetTableByNameOrAlias(facetsConfig, edge), true)
                ).ToList();
            return joins;
        }
    }
}
