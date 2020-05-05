using System.Collections.Generic;
using Autofac.Features.Indexed;
using SeadQueryCore.Model;
using SeadQueryCore.QueryBuilder;

namespace SeadQueryCore.Services.Result
{
    public class MapResultService : DefaultResultService
    {
        public ICategoryCountService CategoryCountService { get; set; }
        private readonly string AggregateKey = "map_result";
           
        public MapResultService(
            IRepositoryRegistry repositoryRegistry,
            IDynamicQueryProxy queryProxy,
            IQuerySetupBuilder querySetupBuilder,
            IResultSqlCompilerLocator sqlCompilerLocator,
            IDiscreteCategoryCountService categoryCountService
        ) : base(repositoryRegistry, queryProxy, querySetupBuilder, sqlCompilerLocator)
        {
            ResultFacetCode = "map_result";
            CategoryCountService = categoryCountService;
        }

        public override ResultContentSet Load(FacetsConfig2 facetsConfig, ResultConfig resultConfig)
        {
            System.Diagnostics.Debug.Assert(resultConfig.AggregateKeys[0] == AggregateKey, "If never false then remove override of load");

            resultConfig.AggregateKeys = new List<string>() { AggregateKey };
            return base.Load(facetsConfig, resultConfig);
        }

        private CategoryCountService.CategoryCountData GetCategoryCounts(FacetsConfig2 facetsConfig)
        {
            return CategoryCountService.Load(ResultFacetCode, facetsConfig, null);
        }

        protected override dynamic GetExtraPayload(FacetsConfig2 facetsConfig)
        {
            var filtered = GetCategoryCounts(facetsConfig)?.CategoryCounts ?? new Dictionary<string, CategoryCountItem>();
            var unfiltered = facetsConfig.HasPicks() ? GetCategoryCounts(facetsConfig.ClearPicks())?.CategoryCounts : filtered;
            return new {
                FilteredCategoryCounts = filtered,
                FullCategoryCounts = unfiltered
            };
        }
    }
}
