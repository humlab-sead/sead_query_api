using System.Collections.Generic;

namespace SeadQueryCore
{
    public interface IEdgeSqlCompiler
    {
        string Compile(IFacetsGraph graph, TableRelation edge, FacetTable facetTable, bool innerJoin = false);
    }
}