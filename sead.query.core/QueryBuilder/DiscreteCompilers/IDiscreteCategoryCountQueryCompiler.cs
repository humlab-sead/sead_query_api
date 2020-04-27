using SeadQueryCore.QueryBuilder;

namespace SeadQueryCore
{
    public interface IDiscreteCategoryCountQueryCompiler : ISqlCompiler
    {
        string Compile(QuerySetup query, Facet facet, Facet countFacet, string aggType);
    }
}