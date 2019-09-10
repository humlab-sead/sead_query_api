using SeadQueryCore.QueryBuilder;

namespace SeadQueryCore
{
    public interface IDiscreteCounterSqlQueryCompiler
    {
        string Compile(QuerySetup query, Facet facet, Facet countFacet, string aggType);
    }
}