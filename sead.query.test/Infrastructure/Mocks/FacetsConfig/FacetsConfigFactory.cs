using SeadQueryCore;
using System.Collections.Generic;
using System.Linq;
using SeadQueryAPI.Serializers;
using System;
using Moq;
using SeadQueryInfra;
using System.Data.Common;

namespace SeadQueryTest.Mocks
{
    internal static class FakeFacetsConfigByUriFactory
    {

        public static FacetsConfig2 Create(string uri)
        {
            return CreateFactory().Create(uri);
        }

        private static FacetsConfigFactory CreateFactory(DbConnection connection=null)
        {
            var context = JsonSeededFacetContextFactory.Create(connection);
            var registry = new RepositoryRegistry(context);
            var factory = new FacetsConfigFactory(registry);
            return factory;
        }

    }

    internal static class FakeFacetsConfigByJsonFactory
    {

        public static FacetsConfig2 Create(string json)
        {
            var registry = FakeFacetsGetByCodeRepositoryFactory.Create();
            var service = new FacetConfigReconstituteService(registry);
            FacetsConfig2 facetsConfig = service.Reconstitute(json);

            return facetsConfig;
        }

    }

    internal static class FakeSingleFacetsConfigFactory
    {

        public static FacetsConfig2 Create(string json)
        {
            var registry = FakeFacetsGetByCodeRepositoryFactory.Create();
            var service = new FacetConfigReconstituteService(registry);
            FacetsConfig2 facetsConfig = service.Reconstitute(json);

            return facetsConfig;
        }

    }

    public class FacetsConfigFactory
    {
        public IRepositoryRegistry Registry { get; private set; }

        public FacetsConfigFactory(IRepositoryRegistry registry)
        {
            Registry = registry;
        }

        public FacetsConfigFactory()
        {
            Registry = FakeFacetsGetByCodeRepositoryFactory.Create();
        }

        private Facet GetFacet(string facetCode) => String.IsNullOrEmpty(facetCode) ? Registry.Facets.GetByCode(facetCode) : null;

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
                    FacetConfigFactory.Create(GetFacet(facetCode), 0, new List<FacetConfigPick>())
                }
            );
        }

        public FacetsConfig2 Create(string uri)
        {
            var config = FacetConfigUriParser.Parse(uri);
            var position = 0;

            var facetConfigs = config
                .FacetCodes
                .Select(z => FacetConfigFactory.Create(GetFacet(z.Key), position++, z.Value))
                .ToList();

            return CreateFacetsConfig(
                config.TargetCode,
                config.TargetCode,
                facetConfigs
            );
        }

    }
}
