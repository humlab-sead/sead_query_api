using System.Data;
using SeadQueryCore.QueryBuilder;

namespace SeadQueryCore;

public interface ICategoryInfoSqlCompiler : ISqlCompiler
{
    CategoryItem ToItem(IDataReader dr);

    string Compile(QuerySetup query, Facet facet, dynamic payload);
}
