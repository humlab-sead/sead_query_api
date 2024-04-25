using SeadQueryCore.QueryBuilder;

namespace SeadQueryCore.Plugin;

public class GeoPolygonCategoryInfoService(IQuerySetupBuilder builder, IGeoPolygonCategoryInfoSqlCompiler compiler) : Common.CategoryInfoService(builder, compiler)
{
}
