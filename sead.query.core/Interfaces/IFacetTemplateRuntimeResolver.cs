namespace SeadQueryCore;

public interface IFacetTemplateRuntimeResolver
{
    FacetTemplateRuntimeSnapshot GetTemplateSnapshot(Facet facet);

    string GetAnchorSql(Facet facet, string anchorTable);

    string GetTemplateKey(Facet facet);

    bool HasAnchorSql(Facet facet, string anchorTable);
}
