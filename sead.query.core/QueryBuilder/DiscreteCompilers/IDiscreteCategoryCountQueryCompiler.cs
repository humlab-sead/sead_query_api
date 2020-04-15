using SeadQueryCore.QueryBuilder;

namespace SeadQueryCore
{
    public interface IDiscreteCategoryCountQueryCompiler
    {
        string Compile(QuerySetup query, Facet facet, Facet countFacet, string aggType);
    }
}