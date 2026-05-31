using System;
using System.Collections.Generic;
using System.Linq;
using SeadQueryCore.Model.Ext;

namespace SeadQueryCore.QueryBuilder;

public interface ISupportedRequestQuerySetupFactory
{
    QuerySetup Create(FacetsConfig2 facetsConfig, Facet targetFacet, List<string> extraTables = null, List<string> facetCodes = null);

    QuerySetup CreateForResultProjection(
        FacetsConfig2 facetsConfig,
        Facet resultFacet,
        IEnumerable<ResultSpecificationField> resultFields
    );
}

public sealed class SupportedRequestQuerySetupFactory : ISupportedRequestQuerySetupFactory
{
    private readonly IPathFinder _pathFinder;
    private readonly IPicksFilterCompiler _picksFilterCompiler;
    private readonly IJoinsClauseCompiler _joinsClauseCompiler;

    public SupportedRequestQuerySetupFactory(
        IPathFinder pathFinder,
        IPicksFilterCompiler picksFilterCompiler,
        IJoinsClauseCompiler joinsClauseCompiler
    )
    {
        _pathFinder = pathFinder ?? throw new ArgumentNullException(nameof(pathFinder));
        _picksFilterCompiler = picksFilterCompiler ?? throw new ArgumentNullException(nameof(picksFilterCompiler));
        _joinsClauseCompiler = joinsClauseCompiler ?? throw new ArgumentNullException(nameof(joinsClauseCompiler));
    }

    public QuerySetup Create(FacetsConfig2 facetsConfig, Facet targetFacet, List<string> extraTables = null, List<string> facetCodes = null)
    {
        ArgumentNullException.ThrowIfNull(facetsConfig);
        ArgumentNullException.ThrowIfNull(targetFacet);

        var involvedConfigs = facetsConfig.GetFacetConfigsAffectedBy(targetFacet, facetCodes);
        if (facetsConfig.HasDomainCode())
        {
            involvedConfigs.Insert(0, facetsConfig.CreateDomainConfig());
        }

        var pickCriterias = _picksFilterCompiler.Compile(targetFacet, involvedConfigs);
        var involvedFacets = involvedConfigs.Facets().AddUnion(targetFacet).ToList();
        var involvedTables = involvedFacets.TableNames().NullableUnion(extraTables).Distinct().ToList();
        var routes = _pathFinder.Find(targetFacet.TargetTable.ResolvedAliasOrTableOrUdfName, involvedTables, true);
        var joins = _joinsClauseCompiler.Compile(routes, facetsConfig);
        var facetCriterias = involvedFacets.Criterias();

        return new QuerySetup
        {
            TargetConfig = facetsConfig.TargetConfig,
            Facet = targetFacet,
            Joins = joins,
            Criterias = pickCriterias.Concat(facetCriterias).ToList(),
        };
    }

    public QuerySetup CreateForResultProjection(
        FacetsConfig2 facetsConfig,
        Facet resultFacet,
        IEnumerable<ResultSpecificationField> resultFields
    )
    {
        if (!(resultFields?.Any() ?? false))
        {
            throw new ArgumentNullException(nameof(resultFields), "ResultConfig is null or is missing specification keys!");
        }

        return Create(
            facetsConfig,
            resultFacet,
            resultFields.GetResultFieldTableNames().ToList(),
            null
        );
    }
}