using System.Collections.Generic;
using System.Linq;

namespace SeadQueryCore
{
    public class FacetsConfigSpecification : QuerySeadSpecification<FacetsConfig2>
    {
        public override bool IsSatisfiedBy(FacetsConfig2 facetsConfig)
        {
            if (!this.IsSatisfiedBy(facetsConfig.FacetConfigs))
            {
                return false;
            }
            if (facetsConfig.RequestId?.Length == 0)
            {
                throw new QuerySeadException("Undefined request id");
            }
            if ((facetsConfig.TargetConfig ?? facetsConfig.TargetConfig) == null)
            {
                throw new QuerySeadException("Target facet is undefined");
            }
            if (facetsConfig.TargetCode != "" && facetsConfig.TargetFacet == null)
            {
                throw new QuerySeadException("Target facet is undefined");
            }
            if (facetsConfig.GetConfig(facetsConfig.TargetCode) == null)
            {
                throw new QuerySeadException("Target facet code invalid (not found in any config)");
            }
            return true;
        }

        public bool IsSatisfiedBy(List<FacetConfig2> configs)
        {
            if (configs.Count == 0)
            {
                throw new QuerySeadException("Facet chain is empty");
            }
            if (configs.Select(z => z.Position).Distinct().Count() != configs.Count)
            {
                throw new QuerySeadException("Facets' positions within facet chain are not unique");
            }
            if (configs.Select(z => z.FacetCode).Distinct().Count() != configs.Count)
            {
                throw new QuerySeadException("Facets' codes within facet chain are not unique");
            }
            if (configs.Any(z => z.Facet == null))
            {
                throw new QuerySeadException("FacetConfig with null Facet not allowed");
            }
            return true;
        }
    }
}
