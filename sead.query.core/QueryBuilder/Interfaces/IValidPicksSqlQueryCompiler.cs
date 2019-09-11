using System.Collections.Generic;
using SeadQueryCore.QueryBuilder;

namespace SeadQueryCore
{
    public interface IValidPicksSqlQueryCompiler
    {
        string Compile(QuerySetup query, Facet facet, List<int> picks);
    }
}