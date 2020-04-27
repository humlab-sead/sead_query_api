using SeadQueryCore.QueryBuilder;
using System.Collections.Generic;

namespace SeadQueryCore
{
    public interface IValidPicksSqlCompiler : ISqlCompiler
    {
        string Compile(QuerySetup query, Facet facet, List<int> picks);
    }
}