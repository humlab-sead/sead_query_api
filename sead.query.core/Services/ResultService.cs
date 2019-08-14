using Autofac.Features.Indexed;
using SeadQueryCore.Model;
using SeadQueryCore.QueryBuilder;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using static SeadQueryCore.Utility;

namespace SeadQueryCore {

    using CatCountDict = Dictionary<string, CategoryCountItem>;

    public interface IResultService {
        ResultContentSet Load(FacetsConfig2 facetsConfig, ResultConfig resultConfig);
    }

    public class DefaultResultService : QueryServiceBase, IResultService {

        public string FacetCode { get; protected set; }

        public IResultQueryCompiler QueryCompiler { get; set; }
        public IIndex<EFacetType, ICategoryCountService> CategoryCountServices { get; set; }

        public DefaultResultService(IQueryBuilderSetting config, IRepositoryRegistry context, IQuerySetupBuilder builder, IResultQueryCompiler compiler, IIndex<EFacetType, ICategoryCountService> categoryCountServices) : base(config, context, builder)
        {
            FacetCode = "result_facet";
            QueryCompiler = compiler;
            CategoryCountServices = categoryCountServices;
        }

        public virtual ResultContentSet Load(FacetsConfig2 facetsConfig, ResultConfig resultConfig)
        {
            string sql = CompileSql(facetsConfig, resultConfig);

            if (empty(sql))
                return null;

            var resultSet = new TabularResultContentSet(resultConfig, GetResultFields(resultConfig), Context.Query(sql)) {
                Payload = GetExtraPayload(facetsConfig),
                Query = sql
            };

            return resultSet;
        }

        protected virtual List<ResultAggregateField> GetResultFields(ResultConfig resultConfig)
        {
            return Context.Results.GetFieldsByKeys(resultConfig.AggregateKeys).Where(z => z.FieldType.IsResultValue).ToList();
        }

        protected virtual dynamic GetExtraPayload(FacetsConfig2 facetsConfig)
        {
            return null;
        }

        protected virtual string CompileSql(FacetsConfig2 facetsConfig, ResultConfig resultConfig)
        {
            return QueryCompiler.Compile(facetsConfig, resultConfig, FacetCode);
        }
    }

    public class MapResultService : DefaultResultService {

        private readonly string ResultKey = "map_result";

        public MapResultService(IQueryBuilderSetting config, IRepositoryRegistry context, IQuerySetupBuilder builder, IResultQueryCompiler resultQueryCompiler, IIndex<EFacetType, ICategoryCountService> categoryCountServices)
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
            return new {
                FilteredCategoryCounts = filtered,
                FullCategoryCounts = unfiltered };
        }

        protected override string CompileSql(FacetsConfig2 facetsConfig, ResultConfig resultConfig)
        {
            return QueryCompiler.Compile(facetsConfig, resultConfig, FacetCode);
        }
    }

}