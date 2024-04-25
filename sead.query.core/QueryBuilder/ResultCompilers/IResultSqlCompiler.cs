using System.Collections.Generic;

namespace SeadQueryCore
{
    public interface IResultSqlCompiler : ISqlCompiler
    {
        string ViewTypeId { get; }
        string Compile(QueryBuilder.QuerySetup query, Facet facet, IEnumerable<ResultSpecificationField> fields);
    }
}
