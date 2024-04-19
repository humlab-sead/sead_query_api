using SeadQueryCore.QueryBuilder;

namespace SeadQueryCore.Plugin;

public class DiscreteCategoryInfoService(IQuerySetupBuilder builder, IDiscreteCategoryInfoSqlCompiler compiler) : Plugin.Common.CategoryInfoService(builder, compiler)
{
}
