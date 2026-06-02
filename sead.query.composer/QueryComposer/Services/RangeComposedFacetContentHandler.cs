using SeadQueryCore;
using SeadQueryCore.Plugin.Range;

namespace SeadQueryComposer.QueryComposer.Services;

public sealed class RangeComposedFacetContentHandler : IntervalComposedFacetContentHandlerBase
{
    public RangeComposedFacetContentHandler(
        ITypedQueryProxy queryProxy,
        SeadQueryCore.QueryComposer.IFacetContentQueryComposer facetContentQueryComposer,
        IRangeCategoryInfoService categoryInfoService
    )
        : base(queryProxy, facetContentQueryComposer, categoryInfoService) { }

    public override EFacetType FacetType => EFacetType.Range;
}
