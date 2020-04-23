using SeadQueryCore.QueryBuilder;

namespace SeadQueryCore
{
    public interface IRangeCategoryCountSqlCompiler
    {
        string Compile(QuerySetup query, Facet facet, string intervalQuery, string countColumn);
    }
}