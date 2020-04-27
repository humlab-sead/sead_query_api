using SeadQueryCore.QueryBuilder;

namespace SeadQueryCore
{
    public interface IRangeCategoryCountSqlCompiler : ISqlCompiler
    {
        string Compile(QuerySetup query, Facet facet, string intervalQuery, string countColumn);
    }
}