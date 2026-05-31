using SeadQueryCore.QueryBuilder;

namespace SeadQueryCore.Plugin.Discrete;

public class DiscreteCategoryInfoService(
    ISupportedRequestQuerySetupFactory factory,
    IDiscreteCategoryInfoSqlCompiler compiler
) : Plugin.Common.CategoryInfoService(factory, compiler), IDiscreteCategoryInfoService
{
}
