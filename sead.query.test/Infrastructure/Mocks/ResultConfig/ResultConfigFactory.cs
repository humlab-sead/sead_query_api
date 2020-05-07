using SeadQueryCore;
using SeadQueryCore.Model;
using System.Collections.Generic;
using System.Linq;

namespace SQT.Mocks
{
    internal static class ResultConfigFactory
    {
        public static ResultConfig Create(Facet facet, List<ResultAggregate> queryAggregates, string viewTypeId)
        {
            var resultConfig =  new ResultConfig()
            {
                Facet = facet,
                FacetCode = facet.FacetCode,
                RequestId = "1",
                SessionId = "1",
                ResultComposites = queryAggregates,
                AggregateKeys = queryAggregates.Select(z => z.AggregateKey).ToList(),
                ViewTypeId = viewTypeId,
            };
            return resultConfig;
        }
        public static ResultConfig Create(Facet facet, ResultAggregate queryAggregate, string viewTypeId)
        {
            return Create(facet, new List<ResultAggregate> { queryAggregate }, viewTypeId);
        }
    }
}
