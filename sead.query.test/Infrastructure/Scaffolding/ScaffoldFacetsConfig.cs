using SeadQueryCore;
using System.Collections.Generic;
using System.Linq;
using SeadQueryInfra;

namespace SeadQueryTest.Fixtures
{
    public class ScaffoldFacetsConfig
    {

        public IRepositoryRegistry Registry { get; set; }

        public ScaffoldFacetsConfig(IRepositoryRegistry registry)
        {
            Registry = registry;
        }

        private Facet GetFacet(string facetCode) => facetCode != "" ? Registry.Facets.GetByCode(facetCode) : null;

        public FacetsConfig2 CreateFacetsConfig(string targetCode, string triggerCode, List<FacetConfig2> facetConfigs)
        {
            return new FacetsConfig2()
            {
                TargetCode = targetCode,
                TargetFacet = GetFacet(targetCode),
                TriggerCode = triggerCode,
                TriggerFacet = GetFacet(triggerCode),
                RequestId = "1",
                Language = "",
                RequestType = "populate",
                FacetConfigs = facetConfigs
            };
        }

        public FacetsConfig2 CreateSingleFacetsConfigWithoutPicks(string facetCode)
        {
            return CreateFacetsConfig(
                facetCode,
                facetCode,
                new List<FacetConfig2>() {
                    ScaffoldFacetConfig.Create(GetFacet(facetCode), 0, new List<FacetConfigPick>())
                }
            );
        }

        public FacetsConfig2 Create(string uri)
        {
            var config = new FacetConfigUriParser().Parse(uri);
            var position = 0;

            var facetConfigs = config
                .FacetCodes
                .Select(z => ScaffoldFacetConfig.Create(GetFacet(z.Key), position++, z.Value))
                .ToList();

            return CreateFacetsConfig(
                config.TargetCode,
                config.TargetCode,
                facetConfigs
            );
        }
    }
}
