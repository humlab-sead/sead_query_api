namespace SeadQueryCore
{
    public interface IPickFilterCompiler : ISqlCompiler
    {
        string Compile(Facet targetFacet, Facet currentFacet, FacetConfig2 config);
    }
}
