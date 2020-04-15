using System.Collections.Generic;
using SeadQueryCore.QueryBuilder;

namespace SeadQueryCore
{
    public interface IValidPicksSqlCompiler
    {
        string Compile(QuerySetup query, Facet facet, List<int> picks);
    }
}