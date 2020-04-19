using SeadQueryCore.QueryBuilder;

namespace SeadQueryCore
{
    public interface IResultSqlCompiler
    {
        string Compile(QueryBuilder.QuerySetup query, Facet facet, ResultQuerySetup resultQuerySetup);
    }
}