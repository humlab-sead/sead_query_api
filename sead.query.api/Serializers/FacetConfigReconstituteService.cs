using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SeadQueryCore;

namespace SeadQueryAPI.Serializers
{
    public class FacetConfigReconstituteService : IFacetConfigReconstituteService
    {
        private readonly IRepositoryRegistry Registry;

        public FacetConfigReconstituteService(IRepositoryRegistry registry)
        {
            Registry = registry;
        }

        public FacetConfig2 Reconstitute(FacetConfig2 facetConfig)
        {
            facetConfig.Facet = Registry.Facets.GetByCode(facetConfig.FacetCode);
            return facetConfig;
        }

        public FacetsConfig2 Reconstitute(FacetsConfig2 facetsConfig)
        {
            facetsConfig.TargetFacet = Registry.Facets.GetByCode(facetsConfig.TargetCode);
            facetsConfig.TriggerFacet = Registry.Facets.GetByCode(facetsConfig.TriggerCode);
            foreach (var config in facetsConfig.FacetConfigs) {
                Reconstitute(config);
            }
            new FacetsConfigSpecification().IsSatisfiedBy(facetsConfig);

            return facetsConfig;
        }

        public FacetsConfig2 Reconstitute(string json)
        {
            var facetsConfig = JsonConvert.DeserializeObject<FacetsConfig2>(json);
            facetsConfig = Reconstitute(facetsConfig);
            return facetsConfig;
        }
        public FacetsConfig2 Reconstitute(JObject json)
        {
            var facetsConfig = Reconstitute(json.ToString());
            return facetsConfig;
        }


    }
}
