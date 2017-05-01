using QueryFacetDomain.QueryBuilder;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using static QueryFacetDomain.Utility;

namespace QueryFacetDomain {

    using CatCountDict = Dictionary<string, CategoryCountValue>;

    public class ResultDataLoader : QueryServiceBase
    {
        public IQuerySetupCompilers CompilerAggregate { get; set; }
        public ResultDataLoader(IQueryBuilderSetting config, IUnitOfWork context, IQuerySetupBuilder builder, IQuerySetupCompilers compilerAggregate) : base(config, context, builder)
        {
            CompilerAggregate = compilerAggregate;
        }

        public dynamic Load(FacetsConfig2 facetsConfig, ResultConfig resultConfig, string facetStateId)
        {
            string sql = CompileSql(facetsConfig, resultConfig);
            if (empty(sql)) {
                return null; // new { Iterator = null, Payload = null };
            }
            IDataReader iterator = Context.Query(sql);
            dynamic payload = GetExtraPayload(facetsConfig, resultConfig);
            return (iterator, payload );
        }

        protected dynamic GetExtraPayload(FacetsConfig2 facetsConfig, ResultConfig resultConfig)
        {
            return null;
        }

        protected string CompileSql(FacetsConfig2 facetsConfig, ResultConfig resultConfig)
        {
            return CompilerAggregate.DefaultQuerySetupCompiler.Compile(facetsConfig, resultConfig);
        }
    }

    class MapResultDataLoader : ResultDataLoader {

        public string facetCode = null;
        public DiscreteCategoryCountService CategoryCountService;

        public MapResultDataLoader(IQueryBuilderSetting config, IUnitOfWork context, IQuerySetupBuilder builder, IQuerySetupCompilers compilerAggregate, DiscreteCategoryCountService categoryCountService) : base(config, context, builder, compilerAggregate)
        {
            facetCode = "map_result";
            CategoryCountService = categoryCountService;
        }


        private Dictionary<string, CategoryCountValue> GetCategoryCounts(FacetsConfig2 facetsConfig)
        {
            return CategoryCountService.Load(facetCode, facetsConfig);
        }

        protected (CatCountDict, CatCountDict) getExtraPayload(FacetsConfig2 facetsConfig, ResultConfig resultConfig)
        {
            CatCountDict data = GetCategoryCounts(facetsConfig);
            CatCountDict filtered = data ?? new Dictionary<string, CategoryCountValue>();
            CatCountDict unfiltered = facetsConfig.HasPicks() ? GetCategoryCounts(facetsConfig.DeletePicks()) : filtered;
            return (filtered, unfiltered);
        }

        protected string compileSql(FacetsConfig2 facetsConfig, ResultConfig resultConfig)
        {
            return CompilerAggregate.MapQuerySetupCompiler.Compile(facetsConfig, null, facetCode);
        }
    }

}