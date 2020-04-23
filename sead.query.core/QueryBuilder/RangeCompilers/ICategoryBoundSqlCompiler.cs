using SeadQueryCore.QueryBuilder;

namespace SeadQueryCore
{

    public interface ICategoryBoundSqlCompiler
    {
        string Compile(QuerySetup query, Facet facet, string facetCode);
    }

}