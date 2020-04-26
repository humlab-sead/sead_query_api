using SeadQueryCore.QueryBuilder;
using System.Collections.Generic;

namespace SeadQueryCore
{
    public interface IResultSqlCompiler
    {
        string Compile(QueryBuilder.QuerySetup query, Facet facet, IEnumerable<ResultAggregateField> fields);
    }
}