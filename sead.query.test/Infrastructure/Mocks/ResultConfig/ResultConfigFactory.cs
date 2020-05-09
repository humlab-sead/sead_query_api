using SeadQueryCore;
using SeadQueryCore.Model;
using System.Collections.Generic;
using System.Linq;

namespace SQT.Mocks
{
    internal static class ResultConfigFactory
    {
        public static ResultConfig Create(Facet facet, List<ResultSpecification> resultSpecifications, string viewTypeId)
        {
            var resultConfig =  new ResultConfig()
            {
                Facet = facet,
                FacetCode = facet.FacetCode,
                RequestId = "1",
                SessionId = "1",
                Specifications = resultSpecifications,
                SpecificationKeys = resultSpecifications.Select(z => z.SpecificationKey).ToList(),
                ViewTypeId = viewTypeId,
            };
            return resultConfig;
        }
        public static ResultConfig Create(Facet facet, ResultSpecification resultSpecification, string viewTypeId)
        {
            return Create(facet, new List<ResultSpecification> { resultSpecification }, viewTypeId);
        }
    }
}
