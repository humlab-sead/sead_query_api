namespace SeadQueryCore;

public interface ICategoryInfoService
{
    FacetContent.CategoryInfo GetCategoryInfo(FacetsConfig2 facetsConfig, string facetCode, dynamic payload = null);

    ICategoryInfoSqlCompiler SqlCompiler { get; }
}
