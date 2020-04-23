using SeadQueryCore.QueryBuilder;

namespace SeadQueryCore
{
    public interface IDiscreteContentSqlCompiler
    {
        string Compile(QuerySetup query, Facet facet, string text_filter);
    }
}