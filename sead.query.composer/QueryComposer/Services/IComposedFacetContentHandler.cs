using System.Collections.Generic;
using SeadQueryCore;
using SeadQueryCore.QueryComposer;

namespace SeadQueryComposer.QueryComposer.Services;

public interface IComposedFacetContentHandler
{
    EFacetType FacetType { get; }

    bool TryValidate(FacetsConfig2 facetsConfig, IReadOnlyList<FacetConfig2> predicateConfigs, out string failureReason);

    string ResolveTargetJoinColumn(Facet targetFacet, string anchorKeyColumn);

    FacetContent Load(FacetsConfig2 facetsConfig, ComposedFacetContentRequest request, ComposedFilterQuery composedFilterQuery);
}
