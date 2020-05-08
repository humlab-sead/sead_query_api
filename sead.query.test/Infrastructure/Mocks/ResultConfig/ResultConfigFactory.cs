using SeadQueryCore;
using SeadQueryCore.Model;
using System.Collections.Generic;
using System.Linq;

namespace SQT.Mocks
{
    internal static class ResultConfigFactory
    {
        public static ResultConfig Create(Facet facet, List<ResultComposite> resultComposites, string viewTypeId)
        {
            var resultConfig =  new ResultConfig()
            {
                Facet = facet,
                FacetCode = facet.FacetCode,
                RequestId = "1",
                SessionId = "1",
                ResultComposites = resultComposites,
                CompositeKeys = resultComposites.Select(z => z.CompositeKey).ToList(),
                ViewTypeId = viewTypeId,
            };
            return resultConfig;
        }
        public static ResultConfig Create(Facet facet, ResultComposite resultComposite, string viewTypeId)
        {
            return Create(facet, new List<ResultComposite> { resultComposite }, viewTypeId);
        }
    }
}
