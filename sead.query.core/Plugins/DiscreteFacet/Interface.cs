using SeadQueryCore.QueryBuilder;
using System.Collections.Generic;

namespace SeadQueryCore.Plugin.Discrete;

public interface IValidPicksSqlCompiler : ISqlCompiler
{
    string Compile(QuerySetup query, List<int> picks);
}

public interface IDiscreteCategoryInfoSqlCompiler : ICategoryInfoSqlCompiler
{
}

public interface IDiscretePickFilterCompiler : IPickFilterCompiler
{
}

public interface IDiscreteCategoryInfoService : ICategoryInfoService
{
}

public interface IDiscreteCategoryCountHelper: ICategoryCountHelper
{
}

public interface IDiscreteCategoryCountSqlCompiler : ICategoryCountSqlCompiler
{
}

public interface IDiscreteFacetPlugin : IFacetPlugin
{
}
