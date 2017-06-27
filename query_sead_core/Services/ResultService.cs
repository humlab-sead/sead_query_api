using QuerySeadDomain.QueryBuilder;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using static QuerySeadDomain.Utility;

namespace QuerySeadDomain {

    using CatCountDict = Dictionary<string, CategoryCountValue>;

    public interface IResultServiceAggregate
    {
        ResultService ResultService { get; set; }
        MapResultService MapResultService { get; set; }
    }
    public class FacetResult : IDisposable {

        public IDataReader Iterator { get; set; }
        public dynamic Payload { get; set; }

        public void Dispose()
        {
            Iterator.Dispose();
        }
    }

    public interface IResultService {
        FacetResult Load(FacetsConfig2 facetsConfig, ResultConfig resultConfig, string facetStateId);
    }

    public class ResultService : QueryServiceBase, IResultService {

        public IQuerySetupCompilers CompilerAggregate { get; set; }

        public ResultService(IQueryBuilderSetting config, IUnitOfWork context, IQuerySetupBuilder builder, IQuerySetupCompilers compilerAggregate) : base(config, context, builder)
        {
            CompilerAggregate = compilerAggregate;
        }

        // FIXME! Använd using runt iterator...?
        public FacetResult Load(FacetsConfig2 facetsConfig, ResultConfig resultConfig, string facetStateId)
        {
            string sql = CompileSql(facetsConfig, resultConfig);
            if (empty(sql)) {
                return null; // new { Iterator = null, Payload = null };
            }
            dynamic payload = GetExtraPayload(facetsConfig, resultConfig);
            return new FacetResult() {
                Iterator = Context.Query(sql),
                Payload = GetExtraPayload(facetsConfig, resultConfig)
            };
        }

        protected virtual dynamic GetExtraPayload(FacetsConfig2 facetsConfig, ResultConfig resultConfig)
        {
            return null;
        }

        protected virtual string CompileSql(FacetsConfig2 facetsConfig, ResultConfig resultConfig)
        {
            return CompilerAggregate.DefaultQuerySetupCompiler.Compile(facetsConfig, resultConfig);
        }
    }

    public class MapResultService : ResultService {

        public string facetCode = "map_result";
        public DiscreteCategoryCountService CategoryCountService;

        public MapResultService(IQueryBuilderSetting config, IUnitOfWork context, IQuerySetupBuilder builder, IQuerySetupCompilers compilerAggregate, DiscreteCategoryCountService categoryCountService) : base(config, context, builder, compilerAggregate)
        {
            CategoryCountService = categoryCountService;
        }

        private Dictionary<string, CategoryCountValue> GetCategoryCounts(FacetsConfig2 facetsConfig)
        {
            return CategoryCountService.Load(facetCode, facetsConfig);
        }

        protected override dynamic GetExtraPayload(FacetsConfig2 facetsConfig, ResultConfig resultConfig)
        {
            CatCountDict data = GetCategoryCounts(facetsConfig);
            CatCountDict filtered = data ?? new Dictionary<string, CategoryCountValue>();
            CatCountDict unfiltered = facetsConfig.HasPicks() ? GetCategoryCounts(facetsConfig.DeletePicks()) : filtered;
            return (filtered, unfiltered);
        }

        protected override string CompileSql(FacetsConfig2 facetsConfig, ResultConfig resultConfig)
        {
            return CompilerAggregate.MapQuerySetupCompiler.Compile(facetsConfig, null, facetCode);
        }
    }

}