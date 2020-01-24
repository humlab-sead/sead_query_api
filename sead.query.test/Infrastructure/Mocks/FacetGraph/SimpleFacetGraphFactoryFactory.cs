using Moq;
using SeadQueryCore;

namespace SeadQueryTest.Mocks
{
    internal static class SimpleFacetGraphFactoryFactory
    {
        public static FacetGraphFactory Create()
        {
            IRepositoryRegistry mockRegistry = SimpleFacetGraphRepositoryRegistryFactory.CreateMockRegistry();
            return new FacetGraphFactory(mockRegistry);
        }
    }


}
