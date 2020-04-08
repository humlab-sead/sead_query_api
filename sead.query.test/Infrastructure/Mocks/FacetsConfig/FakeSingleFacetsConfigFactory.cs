using SeadQueryCore;
using SeadQueryAPI.Serializers;

namespace SeadQueryTest.Mocks
{
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
}
