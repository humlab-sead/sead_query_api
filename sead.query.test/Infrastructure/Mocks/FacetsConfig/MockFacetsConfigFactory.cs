using SeadQueryCore;
using System.Collections.Generic;
using System.Linq;
using System;
using Moq;
using System.Data.Common;
using SeadQueryTest.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace SeadQueryTest.Mocks
{

    public class MockFacetsConfigFactory
    {
        public IRepositoryRegistry Registry { get; private set; }
        private readonly MockFacetConfigUriParser UriParser = new MockFacetConfigUriParser();

        public MockFacetsConfigFactory(IRepositoryRegistry registry)
        {
            Registry = registry;
        }

        private Facet GetFacet(string facetCode)
        {
            if (String.IsNullOrEmpty(facetCode))
                throw new ArgumentNullException(nameof(facetCode), "not allowed");

            return Registry.Facets.GetByCode(facetCode);
        }

        public FacetsConfig2 Create(string targetCode, string triggerCode, List<FacetConfig2> facetConfigs, string requestType = "populate", string domainCode = "")
        {
            return new FacetsConfig2()
            {
                DomainCode = domainCode,
                TargetCode = targetCode,
                TargetFacet = GetFacet(targetCode),
                TriggerCode = triggerCode,
                TriggerFacet = GetFacet(triggerCode),
                RequestId = "1",
                RequestType = requestType,
                FacetConfigs = facetConfigs
            };
        }

        public FacetsConfig2 CreateSingleFacetsConfigWithoutPicks(string facetCode)
        {
            return Create(
                facetCode,
                facetCode,
                new List<FacetConfig2>() {
                    FacetConfigFactory.Create(Registry.Facets.GetByCode(facetCode), 0, new List<FacetConfigPick>())
                }
            );
        }

        public FacetsConfig2 Create(string uri)
        {
            var config = UriParser.Parse(uri);
            var position = 0;

            var facetConfigs = config
                .FacetCodes
                .Select(z => FacetConfigFactory.Create(GetFacet(z.Key), position++, z.Value))
                .ToList();

            return Create(
                config.TargetCode,
                config.TargetCode,
                facetConfigs
            );
        }

    }
}
