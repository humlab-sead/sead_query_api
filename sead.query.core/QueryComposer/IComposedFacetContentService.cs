namespace SeadQueryCore.QueryComposer;

/// <summary>
/// Runs the first composed facet-content path and returns the current facet-content shape.
/// </summary>
public interface IComposedFacetContentService
{
    bool CanHandle(FacetsConfig2 facetsConfig);

    FacetContent Load(FacetsConfig2 facetsConfig);
}
