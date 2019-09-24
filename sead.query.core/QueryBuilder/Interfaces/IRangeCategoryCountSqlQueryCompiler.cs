using SeadQueryCore.QueryBuilder;

namespace SeadQueryCore
{
    public interface IRangeCategoryCountSqlQueryCompiler
    {
        string Compile(QuerySetup query, Facet facet, string intervalQuery, string countColumn);
    }
}