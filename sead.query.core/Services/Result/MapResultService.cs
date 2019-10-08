using System.Collections.Generic;
using Autofac.Features.Indexed;
using SeadQueryCore.Model;
using SeadQueryCore.QueryBuilder;

namespace SeadQueryCore.Services.Result
{
    using CategoryCountItemMap = Dictionary<string, CategoryCountItem>;

    public class MapResultService : DefaultResultService
    {

        public ICategoryCountService CategoryCountService { get; set; }

        private readonly string ResultKey = "map_result";

        public MapResultService(
            IRepositoryRegistry context,
            IResultCompiler resultQueryCompiler,
            IDiscreteCategoryCountService categoryCountService
        )
            : base(context, resultQueryCompiler)
        {
            FacetCode = "map_result";
            CategoryCountService = categoryCountService;
        }

        public override ResultContentSet Load(FacetsConfig2 facetsConfig, ResultConfig resultConfig)
        {
            resultConfig.AggregateKeys = new List<string>() { ResultKey };
            return base.Load(facetsConfig, resultConfig);
        }

        private Dictionary<string, CategoryCountItem> GetCategoryCounts(FacetsConfig2 facetsConfig)
        {
            return CategoryCountService.Load(FacetCode, facetsConfig, null);
        }

        protected override dynamic GetExtraPayload(FacetsConfig2 facetsConfig)
        {
            CategoryCountItemMap data = GetCategoryCounts(facetsConfig);
            CategoryCountItemMap filtered = data ?? new Dictionary<string, CategoryCountItem>();
            CategoryCountItemMap unfiltered = facetsConfig.HasPicks() ? GetCategoryCounts(facetsConfig.ClearPicks()) : filtered;
            return new {
                FilteredCategoryCounts = filtered,
                FullCategoryCounts = unfiltered
            };
        }

        protected override string CompileSql(FacetsConfig2 facetsConfig, ResultConfig resultConfig)
        {
            // TODO This override seems redundant - same call as in base?
            return QueryCompiler.Compile(facetsConfig, resultConfig, FacetCode);
        }
    }

}
