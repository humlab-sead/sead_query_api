namespace SeadQueryCore.QueryBuilder
{
    public interface IPickFilterCompiler
    {
        string Compile(Facet targetFacet, Facet currentFacet, FacetConfig2 config);
    }
}
