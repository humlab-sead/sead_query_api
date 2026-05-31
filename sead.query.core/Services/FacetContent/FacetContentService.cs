using System;
using SeadQueryCore.QueryComposer;

namespace SeadQueryCore;

public class FacetContentService(IComposedFacetContentService composedFacetContentService) : IFacetContentService
{
    public IComposedFacetContentService ComposedFacetContentService { get; } = composedFacetContentService;

    public FacetContent Load(FacetsConfig2 facetsConfig)
    {
        ArgumentNullException.ThrowIfNull(facetsConfig);

        return ComposedFacetContentService.Load(facetsConfig);
    }
}
