using SeadQueryCore.QueryBuilder;

namespace SeadQueryCore
{

    public interface ICategoryBoundSqlQueryCompiler
    {
        string Compile(QuerySetup query, Facet facet, string facetCode);
    }

}