using System.Collections.Generic;

namespace SeadQueryCore
{
    public interface IJoinSqlCompiler
    {
        string Compile(TableRelation edge, FacetTable facetTable, bool innerJoin = false);
    }
}