using SeadQueryCore.QueryBuilder;

namespace SeadQueryCore.Plugin.GeoPolygon;

public class GeoPolygonCategoryInfoService(IQuerySetupBuilder builder, IGeoPolygonCategoryInfoSqlCompiler compiler) : Common.CategoryInfoService(builder, compiler)
{
}
