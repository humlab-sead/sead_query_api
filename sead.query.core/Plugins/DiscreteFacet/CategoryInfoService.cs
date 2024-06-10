using SeadQueryCore.QueryBuilder;

namespace SeadQueryCore.Plugin.Discrete;

public class DiscreteCategoryInfoService(
    IQuerySetupBuilder builder,
    IDiscreteCategoryInfoSqlCompiler compiler
) : Plugin.Common.CategoryInfoService(builder, compiler), IDiscreteCategoryInfoService
{
}
