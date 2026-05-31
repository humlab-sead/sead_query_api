using System;
using System.Linq;
using SeadQueryCore.Model;
using SeadQueryCore.QueryBuilder;

namespace SeadQueryCore.Services.Result
{
    public sealed class LegacyResultProjectionHandoffBuilder : IResultProjectionHandoffBuilder
    {
        private readonly ISupportedRequestQuerySetupFactory _querySetupFactory;

        public LegacyResultProjectionHandoffBuilder(ISupportedRequestQuerySetupFactory querySetupFactory)
        {
            _querySetupFactory = querySetupFactory ?? throw new ArgumentNullException(nameof(querySetupFactory));
        }

        public ResultProjectionHandoff Build(FacetsConfig2 facetsConfig, ResultConfig resultConfig)
        {
            ArgumentNullException.ThrowIfNull(facetsConfig);
            ArgumentNullException.ThrowIfNull(resultConfig);

            var resultFields = resultConfig.GetSortedFields().ToList();
            var querySetup = _querySetupFactory.CreateForResultProjection(facetsConfig, resultConfig.Facet, resultFields);

            return new ResultProjectionHandoff(querySetup, resultFields);
        }
    }
}
