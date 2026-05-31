using System;
using SeadQueryCore.Plugin.Discrete;
using SeadQueryCore.QueryBuilder;

namespace SeadQueryCore;

public interface ISupportedRequestPickSanitizer
{
    FacetsConfig2 Update(FacetsConfig2 facetsConfig);
}

public sealed class SupportedRequestPickSanitizer : ISupportedRequestPickSanitizer
{
    private readonly ISupportedRequestQuerySetupFactory _querySetupFactory;
    private readonly IValidPicksSqlCompiler _picksCompiler;
    private readonly ITypedQueryProxy _queryProxy;

    public SupportedRequestPickSanitizer(
        ISupportedRequestQuerySetupFactory querySetupFactory,
        IValidPicksSqlCompiler picksCompiler,
        ITypedQueryProxy queryProxy
    )
    {
        _querySetupFactory = querySetupFactory ?? throw new ArgumentNullException(nameof(querySetupFactory));
        _picksCompiler = picksCompiler ?? throw new ArgumentNullException(nameof(picksCompiler));
        _queryProxy = queryProxy ?? throw new ArgumentNullException(nameof(queryProxy));
    }

    public FacetsConfig2 Update(FacetsConfig2 facetsConfig)
    {
        ArgumentNullException.ThrowIfNull(facetsConfig);

        foreach (var facetCode in facetsConfig.GetFacetCodes())
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

            var querySetup = _querySetupFactory.Create(facetsConfig, config.Facet);
            config.Picks = _queryProxy.QueryRows(
                _picksCompiler.Compile(querySetup, config.GetIntegerPickValues()),
                reader => new FacetConfigPick(reader.GetString(0), reader.GetString(1))
            );
        }

        return facetsConfig;
    }
}