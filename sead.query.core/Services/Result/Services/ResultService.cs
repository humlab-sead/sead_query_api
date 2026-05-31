using System.Linq;
using SeadQueryCore.Model;
using SeadQueryCore.Model.Ext;

namespace SeadQueryCore.Services.Result
{
    public class ResultService : IResultService
    {
        public IResultProjectionHandoffBuilder ResultProjectionHandoffBuilder { get; }
        public IResultSqlCompilerLocator SqlCompilerLocator { get; }
        public IResultPayloadServiceLocator PayloadServiceLocator { get; }
        public IDynamicQueryProxy QueryProxy { get; }

        public ResultService(
            IDynamicQueryProxy queryProxy,
            IResultProjectionHandoffBuilder resultProjectionHandoffBuilder,
            IResultPayloadServiceLocator payloadServiceLocator,
            IResultSqlCompilerLocator sqlCompilerLocator
        )
        {
            ResultProjectionHandoffBuilder = resultProjectionHandoffBuilder;
            PayloadServiceLocator = payloadServiceLocator;
            SqlCompilerLocator = sqlCompilerLocator;
            QueryProxy = queryProxy;
        }

        public virtual ResultContentSet Load(FacetsConfig2 facetsConfig, ResultConfig resultConfig)
        {
            var handoff = ResultProjectionHandoffBuilder.Build(facetsConfig, resultConfig);

            var compiler = SqlCompilerLocator.Locate(resultConfig.ViewTypeId);
            var sqlQuery = compiler.Compile(handoff.QuerySetup, resultConfig.Facet, handoff.ResultFields);

            return new TabularResultContentSet(
                resultConfig: resultConfig,
                resultFields: handoff.ResultFields.GetResultValueFields().ToList(),
                reader: QueryProxy.Query(sqlQuery) /* This is (for now) only call to generic QueryProxy.Query */
            )
            {
                Payload = GetPayload(facetsConfig, resultConfig),
                Query = sqlQuery,
            };
        }

        private dynamic GetPayload(FacetsConfig2 facetsConfig, ResultConfig resultConfig) =>
            PayloadServiceLocator.Locate(resultConfig.ViewTypeId).GetExtraPayload(facetsConfig, resultConfig.Facet.FacetCode);
    }
}
