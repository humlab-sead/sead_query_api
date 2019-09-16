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
            if (facetsConfig.RequestId == "")
            {
                throw new QuerySeadException("Undefined request id");
            }
            if ((facetsConfig.TargetConfig ?? facetsConfig.TargetConfig) == null)
            {
                throw new QuerySeadException("Target facet is undefined");
            }
            if (facetsConfig.TriggerFacet == null)
            {
                throw new QuerySeadException("Trigger facet is undefined");
            }
            foreach (var facetCode in new List<string>() { facetsConfig.TargetFacet.FacetCode, facetsConfig.TriggerFacet.FacetCode })
            {
                if ( ! facetsConfig.FacetConfigs.Exists(z => z.FacetCode == facetCode))
                {
                    throw new QuerySeadException("Target or trigger facet code invalid (not found in any config)");
                }
            }
            return true;
        }

        public bool IsSatisfiedBy(List<FacetConfig2> configs)
        {
            if (0 == configs.Count)
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
            return true;
        }
    }
}
