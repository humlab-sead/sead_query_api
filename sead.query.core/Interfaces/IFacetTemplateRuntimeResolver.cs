namespace SeadQueryCore;

public interface IFacetTemplateRuntimeResolver
{
    string GetAnchorSql(Facet facet, string anchorTable);

    string GetTemplateKey(Facet facet);

    bool HasAnchorSql(Facet facet, string anchorTable);
}
