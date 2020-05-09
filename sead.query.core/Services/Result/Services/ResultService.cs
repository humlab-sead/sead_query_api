
using SeadQueryCore.Model;
using SeadQueryCore.Model.Ext;
using SeadQueryCore.QueryBuilder;
using System.Collections.Generic;
using System.Linq;

namespace SeadQueryCore.Services.Result
{
    public class ResultService : QueryServiceBase, IResultService
    {
        public IResultSqlCompilerLocator SqlCompilerLocator { get; }
        public IResultPayloadServiceLocator PayloadServiceLocator { get; }
        public IDynamicQueryProxy QueryProxy { get; }

        public ResultService(
            IRepositoryRegistry repositoryRegistry,
            IDynamicQueryProxy queryProxy,
            IQuerySetupBuilder builder,
            IResultPayloadServiceLocator payloadServiceLocator,
            IResultSqlCompilerLocator sqlCompilerLocator
        ) : base(repositoryRegistry, builder)
        {
            PayloadServiceLocator = payloadServiceLocator;
            SqlCompilerLocator = sqlCompilerLocator;
            QueryProxy = queryProxy;
        }

        public virtual ResultContentSet Load(FacetsConfig2 facetsConfig, ResultConfig resultConfig)
        {
            var queryFields = resultConfig.GetSortedFields();

            var querySetup = QuerySetupBuilder
                .Build(facetsConfig, resultConfig.Facet, queryFields);

            // var sqlQuery = resultConfig.ViewType.GetSqlCompiler()
            var sqlQuery = SqlCompilerLocator
                .Locate(resultConfig.ViewTypeId)
                    .Compile(querySetup, resultConfig.Facet, queryFields);

            return new TabularResultContentSet(
                resultConfig: resultConfig,
                resultFields: queryFields.GetResultValueFields().ToList(),
                reader: QueryProxy.Query(sqlQuery) /* This is (for now) only call to generic QueryProxy.Query */
            )
            {
                Payload = GetPayload(facetsConfig, resultConfig),
                Query = sqlQuery
            };
        }

        private dynamic GetPayload(FacetsConfig2 facetsConfig, ResultConfig resultConfig)
            => PayloadServiceLocator.Locate(resultConfig.ViewTypeId)
                .GetExtraPayload(facetsConfig, resultConfig.Facet.FacetCode);
    }
}