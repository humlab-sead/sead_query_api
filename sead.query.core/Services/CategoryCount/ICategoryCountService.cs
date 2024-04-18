using System.Data;

namespace SeadQueryCore;


public interface ICategoryInfoSqlCompiler : ISqlCompiler
{
    CategoryItem ToItem(IDataReader dr);
}

public interface ICategoryInfoService
{
    FacetContent.CategoryInfo GetCategoryInfo(FacetsConfig2 facetsConfig, string facetCode, int default_interval_count = 120);

    ICategoryInfoSqlCompiler SqlCompiler { get; }

}

public interface ICategoryCountService
{
    CategoryCountService.CategoryCountData Load(string facetCode, FacetsConfig2 facetsConfig, EFacetType facetTypeOverride = EFacetType.Unknown);

}

