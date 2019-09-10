using SeadQueryCore.QueryBuilder;

namespace SeadQueryCore
{
    public interface IRangeLowerUpperSqlQueryCompiler
    {
        string Compile(QuerySetup query, Facet facet);
    }
}