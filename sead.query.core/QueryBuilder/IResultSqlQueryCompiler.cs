//using Newtonsoft.Json;
using SeadQueryCore.QueryBuilder;

namespace SeadQueryCore
{
    public interface IResultSqlQueryCompiler
    {
        string Compile(QueryBuilder.QuerySetup query, Facet facet, ResultQuerySetup config);
    }
}