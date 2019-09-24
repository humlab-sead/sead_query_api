using SeadQueryCore.QueryBuilder;

namespace SeadQueryCore
{
    public interface IDiscreteContentSqlQueryCompiler
    {
        string Compile(QuerySetup query, Facet facet, string text_filter);
    }
}