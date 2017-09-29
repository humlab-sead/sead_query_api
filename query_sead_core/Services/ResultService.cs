using Autofac.Features.Indexed;
using QuerySeadDomain.Model;
using QuerySeadDomain.QueryBuilder;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using static QuerySeadDomain.Utility;

namespace QuerySeadDomain {

    using CatCountDict = Dictionary<string, CategoryCountItem>;

    //public interface IResultServiceAggregate
    //{
    //    ResultService ResultService { get; set; }
    //    MapResultService MapResultService { get; set; }
    //}

    public interface IResultService {
        ResultContentSet Load(FacetsConfig2 facetsConfig, ResultConfig resultConfig);
    }

    public class ResultService : QueryServiceBase, IResultService {

        public IResultQueryCompiler ResultQueryCompiler { get; set; }
        public IIndex<EFacetType, ICategoryCountService> CategoryCountServices { get; set; }

        public ResultService(IQueryBuilderSetting config, IUnitOfWork context, IQuerySetupBuilder builder, IResultQueryCompiler resultQueryCompiler, IIndex<EFacetType, ICategoryCountService> categoryCountServices) : base(config, context, builder)
        {
            ResultQueryCompiler = resultQueryCompiler;
            CategoryCountServices = categoryCountServices;
        }

        public virtual ResultContentSet Load(FacetsConfig2 facetsConfig, ResultConfig resultConfig)
        {
            string sql = CompileSql(facetsConfig, resultConfig);
            return empty(sql) ? null : new TabularResultContentSet(
                resultConfig,
                GetResultFields(resultConfig).ToList(),
                Context.Query(sql)) { Payload = GetExtraPayload(facetsConfig) };
        }

        protected virtual IEnumerable<ResultAggregateField> GetResultFields(ResultConfig resultConfig)
        {
            return Context.Results.GetFieldsByKeys(resultConfig.AggregateKeys).Where(z => z.FieldType.IsResultValue);
        }

        protected virtual dynamic GetExtraPayload(FacetsConfig2 facetsConfig)
        {
            return null;
        }

        protected virtual string CompileSql(FacetsConfig2 facetsConfig, ResultConfig resultConfig)
        {
            return ResultQueryCompiler.Compile(facetsConfig, resultConfig);
        }
    }

    public class MapResultService : ResultService {

        public string facetCode = "map_result";
        public string resultKey = "map_result";

        public MapResultService(IQueryBuilderSetting config, IUnitOfWork context, IQuerySetupBuilder builder, IResultQueryCompiler resultQueryCompiler, IIndex<EFacetType, ICategoryCountService> categoryCountServices)
            : base(config, context, builder, resultQueryCompiler, categoryCountServices)
        {
        }

        public override ResultContentSet Load(FacetsConfig2 facetsConfig, ResultConfig resultConfig)
        {
            resultConfig.AggregateKeys = new List<string>() { resultKey };
            return base.Load(facetsConfig, resultConfig);
        }

        private Dictionary<string, CategoryCountItem> GetCategoryCounts(FacetsConfig2 facetsConfig)
        {
            return CategoryCountServices[EFacetType.Discrete].Load(facetCode, facetsConfig, null);
        }

        protected override dynamic GetExtraPayload(FacetsConfig2 facetsConfig)
        {
            CatCountDict data = GetCategoryCounts(facetsConfig);
            CatCountDict filtered = data ?? new Dictionary<string, CategoryCountItem>();
            CatCountDict unfiltered = facetsConfig.HasPicks() ? GetCategoryCounts(facetsConfig.DeletePicks()) : filtered;
            return (filtered, unfiltered);
        }

        protected override string CompileSql(FacetsConfig2 facetsConfig, ResultConfig resultConfig)
        {
            return ResultQueryCompiler.Compile(facetsConfig, resultConfig, facetCode);
        }
    }

}