using System.Data;
using SeadQueryCore.QueryBuilder;

namespace SeadQueryCore
{
    public interface IDiscreteCategoryInfoSqlCompiler : ICategoryInfoSqlCompiler
    {
        string Compile(QuerySetup query, Facet facet, string text_filter);
    }
}
