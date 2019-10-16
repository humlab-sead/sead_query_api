using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using SeadQueryCore;

namespace SeadQueryAPI.Serializers
{
    public class FacetConfigReconstituteService : IFacetConfigReconstituteService
    {
        private readonly IRepositoryRegistry Registry;
        private readonly JsonSerializerSettings SerializerSettings;

        public FacetConfigReconstituteService(IRepositoryRegistry registry)
        {
            Registry = registry;
            SerializerSettings = new JsonSerializerSettings {
                Error = (sender, errorArgs) =>
                {
                    var currentError = errorArgs.ErrorContext.Error.Message;
                    errorArgs.ErrorContext.Handled = true;
                }
            };
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
            FacetsConfig2 facetsConfig = default(FacetsConfig2);

            var enclosedFacetsConfigsObject = JObject.Parse(json).SelectToken("FacetsConfig");
            if (enclosedFacetsConfigsObject != null) {
                var enclosedJson = enclosedFacetsConfigsObject.ToString();
                facetsConfig = JsonConvert.DeserializeObject<FacetsConfig2>(enclosedJson, SerializerSettings);
            } else {
                Debug.Assert(JObject.Parse(json).SelectToken("TargetCode") != null);
                facetsConfig = JsonConvert.DeserializeObject<FacetsConfig2>(json, SerializerSettings);
            }
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
