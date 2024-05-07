using SeadQueryCore.QueryBuilder;
using System.Collections.Generic;

namespace SeadQueryCore.Plugin.GeoPolygon;

public interface IValidPicksSqlCompiler : ISqlCompiler
{
    string Compile(QuerySetup query, List<int> picks);
}

public interface IGeoPolygonCategoryInfoSqlCompiler : ICategoryInfoSqlCompiler
{
}

public interface IGeoPolygonPickFilterCompiler : IPickFilterCompiler
{
}

public interface IGeoPolygonCategoryInfoService : ICategoryInfoService
{
}

public interface IGeoPolygonCategoryCountHelper: Discrete.IDiscreteCategoryCountHelper
{
}

public interface IGeoPolygonCategoryCountSqlCompiler : ICategoryCountSqlCompiler
{
}

public interface IGeoPolygonFacetPlugin : IFacetPlugin
{
}
