using SeadQueryCore.QueryBuilder;

namespace SeadQueryCore
{
    public interface IResultQuerySetupSqlCompiler
    {
        string Compile(QueryBuilder.QuerySetup query, Facet facet, ResultQuerySetup resultQuerySetup);
    }
}