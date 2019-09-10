using SeadQueryCore.QueryBuilder;

namespace SeadQueryCore
{
    public interface IRangeCounterSqlQueryCompiler
    {
        string Compile(QuerySetup query, Facet facet, string intervalQuery, string countColumn);
    }
}