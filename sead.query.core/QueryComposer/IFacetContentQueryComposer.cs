namespace SeadQueryCore.QueryComposer;

/// <summary>
/// Builds a target facet-content query from the composed anchor-filter query.
/// </summary>
public interface IFacetContentQueryComposer
{
    FacetContentQueryPlan Compose(FacetsConfig2 facetsConfig, ComposedFilterQuery composedFilterQuery);
}