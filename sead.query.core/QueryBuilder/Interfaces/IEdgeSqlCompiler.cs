using System.Collections.Generic;

namespace SeadQueryCore
{
    public interface IEdgeSqlCompiler
    {
        string Compile(TableRelation edge, FacetTable facetTable, Dictionary<string, FacetTable> aliases, bool innerJoin = false);
    }
}