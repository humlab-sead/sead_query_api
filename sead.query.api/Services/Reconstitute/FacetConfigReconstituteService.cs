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
    public class FacetConfigReconstituteService : ReconstituteService, IFacetConfigReconstituteService
    {
        public FacetConfigReconstituteService(IRepositoryRegistry registry) : base(registry)
        {
        }

        public FacetConfig2 Reconstitute(FacetConfig2 facetConfig)
        {
            facetConfig.Facet = GetFacetByCode(facetConfig.FacetCode);
            return facetConfig;
        }

        public FacetsConfig2 Reconstitute(FacetsConfig2 facetsConfig)
        {
            Debug.Assert(facetsConfig.TargetCode != null);
            if (Utility.empty(facetsConfig.TriggerCode)) {
                facetsConfig.TriggerCode = facetsConfig.TargetCode;
            }
            facetsConfig.DomainFacet = GetFacetByCode(facetsConfig.DomainCode);
            facetsConfig.TargetFacet = GetFacetByCode(facetsConfig.TargetCode);

            foreach (var config in facetsConfig.FacetConfigs) {
                Reconstitute(config);
            }

            // if (!String.IsNullOrEmpty(facetsConfig.DomainCode)) {
            //     facetsConfig.FacetConfigs.Insert(0, CreateConfigByCode(facetsConfig.DomainCode));
            // }

            new FacetsConfigSpecification().IsSatisfiedBy(facetsConfig);

            return facetsConfig;
        }

        private FacetConfig2 CreateConfigByCode(string facetCode)
        {
            return FacetConfigFactory.Create(GetFacetByCode(facetCode), 0);
        }

        public FacetsConfig2 Reconstitute(string json)
        {
            FacetsConfig2 facetsConfig = default(FacetsConfig2);

            var enclosedFacetsConfigsObject = JObject.Parse(json).SelectToken("FacetsConfig");
            if (enclosedFacetsConfigsObject != null) {
                var enclosedJson = enclosedFacetsConfigsObject.ToString();
                facetsConfig = JsonConvert.DeserializeObject<FacetsConfig2>(enclosedJson, SerializerSettings);
            } else {
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
