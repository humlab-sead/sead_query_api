using System.Collections.Generic;

namespace SeadQueryCore.QueryBuilder
{
    public interface IPicksFilterCompiler
    {
        IEnumerable<string> Compile(Facet targetFacet, List<FacetConfig2> involvedConfigs);
    }
}
