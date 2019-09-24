using System.Collections.Generic;
using Autofac.Features.Indexed;
using SeadQueryCore.Model;
using SeadQueryCore.QueryBuilder;

namespace SeadQueryCore.Services.Result
{
    using CategoryCountItemMap = Dictionary<string, CategoryCountItem>;

    public class MapResultService : DefaultResultService
    {


        private readonly string ResultKey = "map_result";

        public MapResultService(
            IRepositoryRegistry context,
            IResultCompiler resultQueryCompiler,
            IIndex<EFacetType, ICategoryCountService> categoryCountServices
        )
            : base(context, resultQueryCompiler, categoryCountServices)
        {
            FacetCode = "map_result";
        }

        public override ResultContentSet Load(FacetsConfig2 facetsConfig, ResultConfig resultConfig)
        {
            resultConfig.AggregateKeys = new List<string>() { ResultKey };
            return base.Load(facetsConfig, resultConfig);
        }

        private Dictionary<string, CategoryCountItem> GetCategoryCounts(FacetsConfig2 facetsConfig)
        {
            return CategoryCountServices[EFacetType.Discrete].Load(FacetCode, facetsConfig, null);
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
            return QueryCompiler.Compile(facetsConfig, resultConfig, FacetCode);
        }
    }

}
