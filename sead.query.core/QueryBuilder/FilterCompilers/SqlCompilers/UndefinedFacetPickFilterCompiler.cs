using System;

namespace SeadQueryCore.QueryBuilder
{
    public class UndefinedFacetPickFilterCompiler : IPickFilterCompiler
    {
        public string Compile(Facet targetFacet, Facet currentFacet, FacetConfig2 config)
        {
            throw new ArgumentException();
        }
    }
}
