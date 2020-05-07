using System.Collections.Generic;

namespace SeadQueryCore.QueryBuilder
{
    public interface IJoinsClauseCompiler
    {
        List<string> Compile(FacetsConfig2 facetsConfig, Facet targetFacet, List<string> involvedTables);
    }
}