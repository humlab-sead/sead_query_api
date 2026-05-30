using System;
using System.Linq;
using SeadQueryCore.Model;
using SeadQueryCore.QueryBuilder;

namespace SeadQueryCore.Services.Result
{
    public sealed class LegacyResultProjectionHandoffBuilder : IResultProjectionHandoffBuilder
    {
        private readonly IQuerySetupBuilder _querySetupBuilder;

        public LegacyResultProjectionHandoffBuilder(IQuerySetupBuilder querySetupBuilder)
        {
            _querySetupBuilder = querySetupBuilder ?? throw new ArgumentNullException(nameof(querySetupBuilder));
        }

        public ResultProjectionHandoff Build(FacetsConfig2 facetsConfig, ResultConfig resultConfig)
        {
            ArgumentNullException.ThrowIfNull(facetsConfig);
            ArgumentNullException.ThrowIfNull(resultConfig);

            var resultFields = resultConfig.GetSortedFields().ToList();
            var querySetup = _querySetupBuilder.Build(facetsConfig, resultConfig.Facet, resultFields);

            return new ResultProjectionHandoff(querySetup, resultFields);
        }
    }
}
