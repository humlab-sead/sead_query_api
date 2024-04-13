using System.Collections.Generic;
using SeadQueryCore.Model;
using SeadQueryCore.QueryBuilder;

namespace SeadQueryCore.Services.Result
{
    public class CategoryCountPayloadService : IResultPayloadService
    {
        public ICategoryCountService CategoryCountService { get; set; }

        public CategoryCountPayloadService(ICategoryCountService categoryCountService)
        {
            CategoryCountService = categoryCountService;
        }

        private CategoryCountService.CategoryCountData GetCategoryCounts(FacetsConfig2 facetsConfig, string resultFacetCode)
        {
            return CategoryCountService.Load(resultFacetCode, facetsConfig, null, EFacetType.Discrete);
        }

        public dynamic GetExtraPayload(FacetsConfig2 facetsConfig, string resultFacetCode)
        {
            var filtered = GetCategoryCounts(facetsConfig, resultFacetCode)?.CategoryCounts ?? new Dictionary<string, CategoryCountItem>();
            var unfiltered = facetsConfig.HasPicks() ? GetCategoryCounts(facetsConfig.ClearPicks(), resultFacetCode)?.CategoryCounts : filtered;
            return new
            {
                FilteredCategoryCounts = filtered,
                FullCategoryCounts = unfiltered
            };
        }
    }
}
