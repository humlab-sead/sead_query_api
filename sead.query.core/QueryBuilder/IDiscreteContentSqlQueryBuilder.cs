using SeadQueryCore.QueryBuilder;

namespace SeadQueryCore
{
    public interface IDiscreteContentSqlQueryBuilder
    {
        string Compile(QuerySetup query, Facet facet, string text_filter);
    }
}