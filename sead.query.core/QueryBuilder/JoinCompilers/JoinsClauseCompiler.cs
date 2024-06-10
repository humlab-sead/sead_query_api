using System.Collections.Generic;
using System.Linq;

namespace SeadQueryCore.QueryBuilder
{
    public class JoinsClauseCompiler(IRouteFinder finder, IJoinSqlCompiler joinCompiler) : IJoinsClauseCompiler
    {
        public IJoinSqlCompiler JoinCompiler { get; } = joinCompiler;
        public IRouteFinder Finder { get; set; } = finder;

        private FacetTable GetFacetTableByNameOrAlias(FacetsConfig2 facetsConfig, TableRelation edge)
            => facetsConfig.GetFacetTable(edge.TargetName) ?? GetByAlias(edge.TargetName);

        public virtual List<string> Compile(FacetsConfig2 facetsConfig, Facet targetFacet, List<string> involvedTables)
        {
            var routes = Finder.Find(targetFacet.TargetTable.ResolvedAliasOrTableOrUdfName, involvedTables, true);
            var joins = routes.Edges().Select(
                    edge => JoinCompiler.Compile(edge, GetFacetTableByNameOrAlias(facetsConfig, edge), true)
                ).ToList();
            return joins;
        }

        private FacetTable GetByAlias(string aliasName) => Finder.Registry.FacetTables.GetByAlias(aliasName);

    }
}
