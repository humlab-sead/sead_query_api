
using SeadQueryCore.Model;
using System.Collections.Generic;
using System.Linq;

namespace SeadQueryCore.Services.Result
{

    public class DefaultResultService : IResultService
    {
        public IRepositoryRegistry RepositoryRegistry { get; set; }

        public string FacetCode { get; protected set; }

        public IResultConfigCompiler ResultConfigCompiler { get; set; }
        public IDynamicQueryProxy QueryProxy { get; }

        public DefaultResultService(
            IRepositoryRegistry registry,
            IResultConfigCompiler compiler,
            IDynamicQueryProxy queryProxy
        )
        {
            RepositoryRegistry = registry;
            FacetCode = "result_facet";
            ResultConfigCompiler = compiler;
            QueryProxy = queryProxy;
        }

        public virtual ResultContentSet Load(FacetsConfig2 facetsConfig, ResultConfig resultConfig)
        {
            string sql = CompileSql(facetsConfig, resultConfig);

            if (Utility.empty(sql))
                return null;

            // This is (for now) only call to generic QueryProxy.Query
            var fields = GetResultFields(resultConfig);
            var reader = QueryProxy.Query(sql);
            var resultSet = new TabularResultContentSet(resultConfig, fields, reader) {
                Payload = GetExtraPayload(facetsConfig),
                Query = sql
            };

            return resultSet;
        }

        protected virtual List<ResultAggregateField> GetResultFields(ResultConfig resultConfig)
        {
            return RepositoryRegistry.Results
                    .GetFieldsByKeys(resultConfig.AggregateKeys)
                    .Where(z => z.FieldType.IsResultValue).ToList();
        }

        protected virtual dynamic GetExtraPayload(FacetsConfig2 facetsConfig)
        {
            // TODO Check if this really always should be null for tabular results
            return null;
        }

        protected virtual string CompileSql(FacetsConfig2 facetsConfig, ResultConfig resultConfig)
        {
            return ResultConfigCompiler.Compile(facetsConfig, resultConfig, FacetCode /* result_facet */);
        }
    }

}