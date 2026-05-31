using SeadQueryCore.QueryBuilder;

namespace SeadQueryCore.Plugin.GeoPolygon;

public class GeoPolygonCategoryInfoService(ISupportedRequestQuerySetupFactory factory, IGeoPolygonCategoryInfoSqlCompiler compiler)
    : Common.CategoryInfoService(factory, compiler),
        IGeoPolygonCategoryInfoService { }
