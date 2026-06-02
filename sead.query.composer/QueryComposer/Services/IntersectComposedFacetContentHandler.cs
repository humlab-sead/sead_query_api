using SeadQueryCore;
using SeadQueryCore.Plugin.Intersect;

namespace SeadQueryComposer.QueryComposer.Services;

public sealed class IntersectComposedFacetContentHandler : IntervalComposedFacetContentHandlerBase
{
    public IntersectComposedFacetContentHandler(
        ITypedQueryProxy queryProxy,
        SeadQueryCore.QueryComposer.IFacetContentQueryComposer facetContentQueryComposer,
        IIntersectCategoryInfoService categoryInfoService
    )
        : base(queryProxy, facetContentQueryComposer, categoryInfoService) { }

    public override EFacetType FacetType => EFacetType.Intersect;
}
