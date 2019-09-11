using SeadQueryCore.QueryBuilder;

namespace SeadQueryCore
{
    public interface IDiscreteCategoryCountSqlQueryCompiler
    {
        string Compile(QuerySetup query, Facet facet, Facet countFacet, string aggType);
    }
}