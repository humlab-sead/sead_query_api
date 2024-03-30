using System;

namespace SeadQueryCore.QueryBuilder;
public class UndefinedPickFilterCompiler : IPickFilterCompiler
{
    public string Compile(Facet targetFacet, Facet currentFacet, FacetConfig2 config)
    {
        throw new ArgumentException();
    }
}
