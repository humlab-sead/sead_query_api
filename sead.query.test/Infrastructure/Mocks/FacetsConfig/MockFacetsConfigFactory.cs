using SeadQueryCore;
using System.Collections.Generic;
using System.Linq;
using System;
using Moq;
using System.Data.Common;
using SQT.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace SQT.Mocks
{
    public class MockFacetsConfigFactory
    {
        public IFacetRepository Repository { get; private set; }
        private readonly MockFacetConfigUriParser UriParser = new MockFacetConfigUriParser();

        public MockFacetsConfigFactory(IFacetRepository repository)
        {
            Repository = repository;
        }

        private Facet GetFacet(string facetCode)
        {
            if (String.IsNullOrEmpty(facetCode))
                throw new ArgumentNullException(nameof(facetCode), "not allowed");

            return Repository.GetByCode(facetCode);
        }

        public FacetsConfig2 Create(string targetCode, List<FacetConfig2> facetConfigs, string requestType = "populate", string domainCode = "")
        {
            return new FacetsConfig2()
            {
                DomainCode = domainCode,
                DomainFacet = domainCode.IsNotEmpty() ? GetFacet(domainCode) : null,
                TargetCode = targetCode,
                TargetFacet = GetFacet(targetCode),
                RequestId = "1",
                RequestType = requestType,
                FacetConfigs = facetConfigs
            };
        }

        public FacetsConfig2 CreateSingleFacetsConfigWithoutPicks(string facetCode)
        {
            return Create(
                facetCode,
                [
                    MockFacetConfigFactory.Create(Repository.GetByCode(facetCode), 0, new List<FacetConfigPick>())
                ]
            );
        }

        /// <summary>
        /// Creates a new FacesConfig instance based on given Uri template.
        ///     "target-facet[@trigger-facet]:(facet-code[@picks])(/facet-code[@picks])*"
        /// </summary>
        /// <param name="uri">
        /// String specification in format:
        ///
        ///   "target-facet[@trigger-facet]:(facet-code[@picks])(/facet-code[@picks])*"
        ///
        /// </param>
        /// <returns></returns>
        public FacetsConfig2 Create(string uri)
        {
            var config = UriParser.Parse(uri);
            var position = 0;

            var facetConfigs = config
                .FacetCodes
                .Select(z => MockFacetConfigFactory.Create(GetFacet(z.Key), position++, z.Value))
                .ToList();

            return Create(
                config.TargetCode,
                facetConfigs,
                "populate",
                config.Domain
            );
        }
    }
}
