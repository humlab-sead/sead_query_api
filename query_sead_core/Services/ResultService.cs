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

    public class DefaultResultService : QueryServiceBase, IResultService {

        public string FacetCode { get; protected set; }

        public IResultQueryCompiler ResultQueryCompiler { get; set; }
        public IIndex<EFacetType, ICategoryCountService> CategoryCountServices { get; set; }

        public DefaultResultService(IQueryBuilderSetting config, IUnitOfWork context, IQuerySetupBuilder builder, IResultQueryCompiler resultQueryCompiler, IIndex<EFacetType, ICategoryCountService> categoryCountServices) : base(config, context, builder)
        {
            FacetCode = "result_facet";
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
            return ResultQueryCompiler.Compile(facetsConfig, resultConfig, FacetCode);
        }
    }

    public class MapResultService : DefaultResultService {

        private readonly string ResultKey = "map_result";

        public MapResultService(IQueryBuilderSetting config, IUnitOfWork context, IQuerySetupBuilder builder, IResultQueryCompiler resultQueryCompiler, IIndex<EFacetType, ICategoryCountService> categoryCountServices)
            : base(config, context, builder, resultQueryCompiler, categoryCountServices)
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
            CatCountDict data = GetCategoryCounts(facetsConfig);
            CatCountDict filtered = data ?? new CatCountDict();
            CatCountDict unfiltered = facetsConfig.HasPicks() ? GetCategoryCounts(facetsConfig.DeletePicks()) : filtered;
            return (filtered, unfiltered);
        }

        protected override string CompileSql(FacetsConfig2 facetsConfig, ResultConfig resultConfig)
        {
            return ResultQueryCompiler.Compile(facetsConfig, resultConfig, FacetCode);
        }
    }

}