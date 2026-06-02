namespace SeadQueryComposer.QueryComposer.Services;

public interface IComposedFacetContentRequestFactory
{
    bool TryCreate(
        SeadQueryCore.FacetsConfig2 facetsConfig,
        IComposedFacetContentHandler handler,
        out ComposedFacetContentRequest request,
        out string failureReason
    );

    ComposedFacetContentRequest Create(SeadQueryCore.FacetsConfig2 facetsConfig, IComposedFacetContentHandler handler);
}
