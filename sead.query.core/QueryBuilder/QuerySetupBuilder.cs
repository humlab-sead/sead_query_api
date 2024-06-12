using SeadQueryCore.Model.Ext;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SeadQueryCore.QueryBuilder;

using Route = List<TableRelation>;

public class QuerySetupBuilder : IQuerySetupBuilder
{
    public IRouteFinder RouteFinder { get; set; }
    public IPicksFilterCompiler PicksCompiler { get; set; }
    public IJoinsClauseCompiler JoinCompiler { get; }

    public QuerySetupBuilder(IRouteFinder graph, IPicksFilterCompiler picksCompiler, IJoinsClauseCompiler joinCompiler)
    {
        RouteFinder = graph;
        PicksCompiler = picksCompiler;
        JoinCompiler = joinCompiler;
    }

    /// <summary>
    /// This the core of the query builder that builds SQL query components for a given target facet, and the
    /// chain of facet configurations preceeding the target facet.
    /// Rules:
    ///   - Collect/compile users pick constraint for all involved facets current facet
    ///      - All picks *predeeding* target facet should be included
    ///      - If target is a "discrete" facet then the facet's own constraints should NOT be included.
    ///      - If target facet is "range" then the facet's own constraints should be included,
    ///          but the bound should be expanded to the facet's entire range.
    ///   - Get all selection preceding the target facet.
    ///   - Make where-clauses depending on  type of facets (range or discrete)
    /// </summary>
    /// <param name="facetsConfig">The (complete) facet configuration, including user picks</param>
    /// <param name="targetFacet">Target facet that the query populates/computes/counts etc</param>
    /// <param name="extraTables">,Any extra tables that should be part of the query (path to these tables are included in final query)</param>
    /// <param name="facetCodes">FIXME: "The list of the facets in the view-state"</param>
    /// <returns>
    /// QuerySetup:
    ///     TargetConfig    Reference to targets facet's configuration              (only added for convenience)
    ///     Facet           Reference to target facet                               (only added for convenience)
    ///     Routes          Compiled routes                                         (added for easier testing purposes)
    ///     Joins           Join clauses to be used in final SQL
    ///     Criterias       Criteria clauses to be included in final SQL
    /// </returns>
    public QuerySetup Build(FacetsConfig2 facetsConfig, Facet targetFacet, List<string> extraTables, List<string> facetCodes)
    {
        // Noteworthy: TargetFacet differs from facetsConfig.TargetFacet when a result-facet is applied

        var involvedConfigs = facetsConfig.GetFacetConfigsAffectedBy(targetFacet, facetCodes);

        if (facetsConfig.HasDomainCode())
            involvedConfigs.Insert(0, facetsConfig.CreateDomainConfig());

        var pickCriterias = PicksCompiler.Compile(targetFacet, involvedConfigs);
        var involvedFacets = involvedConfigs.Facets().AddUnion(targetFacet).ToList();
        var involvedTables = involvedFacets.TableNames().NullableUnion(extraTables).Distinct().ToList();
        var routes = GetRoutes(targetFacet, involvedTables);

        var involvedJoins = JoinCompiler.Compile(routes, facetsConfig);
        var facetCriterias = involvedFacets.Criterias();

        QuerySetup querySetup = new QuerySetup()
        {
            TargetConfig = facetsConfig?.TargetConfig,
            Facet = targetFacet,
            Joins = involvedJoins,
            Criterias = pickCriterias.Concat(facetCriterias).ToList()
        };

        return querySetup;
    }

    private List<Route> GetRoutes(Facet targetFacet, List<string> involvedTables)
    {
        return RouteFinder.Find(targetFacet.TargetTable.ResolvedAliasOrTableOrUdfName, involvedTables, true);
    }

    public QuerySetup Build(FacetsConfig2 facetsConfig, Facet resultFacet, IEnumerable<ResultSpecificationField> resultFields)
    {
        if (!resultFields?.Any() ?? false)
            throw new ArgumentNullException($"ResultConfig is null or is missing specification keys!");

        /* All tables referenced by the result fields must be included in query */
        return Build(
            facetsConfig: facetsConfig,
            targetFacet: resultFacet,
            extraTables: resultFields.GetResultFieldTableNames().ToList(),
            facetCodes: null
        );
    }
}
