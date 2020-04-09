using SeadQueryCore;
using SeadQueryInfra;

namespace SeadQueryTest.Mocks
{
    internal static class JsonSeededFacetsConfigByUriFactory
    {

        public static FacetsConfig2 Create(FacetContext context, string uri)
        {
            var registry = new RepositoryRegistry(context);
            var factory = new MockFacetsConfigFactory(registry.Facets);
            return factory.Create(uri);
        }

    }
}
