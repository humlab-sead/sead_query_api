using SeadQueryCore;
using System.Collections.Generic;
using System.Linq;
using SeadQueryInfra;

namespace SeadQueryTest.fixtures
{
    public class FacetConfigGenerator
    {

        public FacetConfigFixtureData Data { get; set; }
        public IRepositoryRegistry Registry { get; set; }
        public IFacetRepository Facets{ get; set; }

        // public FacetConfigGenerator(IContainer container, IFacetContext context)
        public FacetConfigGenerator(RepositoryRegistry registry)
        {
            Data = new FacetConfigFixtureData();
            Registry = registry;
            Facets = Registry.Facets;
        }

        private Facet GetFacet(string facetCode) => facetCode != "" ? Registry.Facets.GetByCode(facetCode) : null;

        public FacetsConfig2 GenerateFacetsConfig(string targetCode, string triggerCode, List<FacetConfig2> facetConfigs)
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

        public FacetConfig2 GenerateFacetConfig(string facetCode, int position, List<FacetConfigPick> picks = null, string filter = "")
        {
            return new FacetConfig2()
            {
                FacetCode = facetCode,
                Facet = GetFacet(facetCode),
                Position = position,
                TextFilter = filter,
                Picks = picks ?? new List<FacetConfigPick>()
            };
        }

        public List<FacetConfigPick> GenerateDiscreteFacetPicks(List<int> ids)
        {
            return ids.Select(z => new FacetConfigPick(EPickType.discrete, z.ToString(), z.ToString())).ToList();
        }

        public FacetsConfig2 GenerateSingleFacetsConfigWithoutPicks(string facetCode)
        {
            return GenerateFacetsConfig(facetCode, facetCode, new List<FacetConfig2>() { GenerateFacetConfig(facetCode, 0, new List<FacetConfigPick>()) });
        }

        public FacetsConfig2 GenerateByConfig(FacetConfigURI config)
        {
            var position = 0;
            var facetConfigs = config.FacetCodes.Select(z => GenerateFacetConfig(z.Key, position++, z.Value)).ToList();
            return GenerateFacetsConfig(config.TargetCode, config.TargetCode, facetConfigs);
        }

        public FacetsConfig2 GenerateByUri(string uri, List<List<string>> expectedTrails = null, int expectedCount = 0)
        {
            return GenerateByConfig(new FacetConfigURI(uri, expectedTrails, expectedCount));
        }

    }
}
