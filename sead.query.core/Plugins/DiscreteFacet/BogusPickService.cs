using SeadQueryCore.QueryBuilder;

namespace SeadQueryCore.Plugin.Discrete;

public class BogusPickService : IBogusPickService
{
    public BogusPickService(
        ISupportedRequestQuerySetupFactory querySetupFactory,
        IValidPicksSqlCompiler picksCompiler,
        ITypedQueryProxy queryProxy
    )
    {
        QuerySetupFactory = querySetupFactory;
        PicksCompiler = picksCompiler;
        QueryProxy = queryProxy;
    }

    public ISupportedRequestQuerySetupFactory QuerySetupFactory { get; }
    public IValidPicksSqlCompiler PicksCompiler { get; }
    public ITypedQueryProxy QueryProxy { get; }

    /// <summary>
    /// Removes invalid selections e.g. hidden selections still being sent from the client.
    /// The client keep them since they can be visible when the filters changes.
    /// This is only applicable for discrete facets (range facet selection are always visible)
    /// </summary>
    /// <param name="facetsConfig"></param>
    /// <returns></returns>
    public FacetsConfig2 Update(FacetsConfig2 facetsConfig)
    {
        foreach (string facetCode in facetsConfig.GetFacetCodes())
        {
            var config = facetsConfig.GetConfig(facetCode);

            if (config.Facet.FacetTypeId != EFacetType.Discrete || config.Picks.Count == 0)
            {
                continue;
            }

            if (!config.HasPicks())
            {
                continue;
            }

            config.Picks = QueryProxy.QueryRows(
                PicksCompiler.Compile(
                    QuerySetupFactory.Create(facetsConfig, config.Facet),
                    config.GetIntegerPickValues()
                ),
                x => new FacetConfigPick(x.GetString(0), x.GetString(1))
            );
        }
        return facetsConfig;
    }
}
