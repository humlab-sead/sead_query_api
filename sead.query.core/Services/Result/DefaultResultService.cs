
using System.Collections.Generic;
using System.Linq;
using Autofac.Features.Indexed;
using SeadQueryCore.Model;
using SeadQueryCore.QueryBuilder;

namespace SeadQueryCore.Services.Result {

    public class DefaultResultService : QueryServiceBase, IResultService
    {
        public string FacetCode { get; protected set; }

        public IResultCompiler QueryCompiler { get; set; }
        public IIndex<EFacetType, ICategoryCountService> CategoryCountServices { get; set; }

        public DefaultResultService(IQueryBuilderSetting config, IRepositoryRegistry context, IQuerySetupBuilder builder, IResultCompiler compiler, IIndex<EFacetType, ICategoryCountService> categoryCountServices) : base(config, context, builder)
        {
            FacetCode = "result_facet";
            QueryCompiler = compiler;
            CategoryCountServices = categoryCountServices;
        }

        public virtual ResultContentSet Load(FacetsConfig2 facetsConfig, ResultConfig resultConfig)
        {
            string sql = CompileSql(facetsConfig, resultConfig);

            if (Utility.empty(sql))
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

}