using Moq;
using SeadQueryCore;

namespace SQT.Mocks
{
    internal static class SimpleRouteFinderFactory
    {
        public static RouteFinder Create()
        {
            IRepositoryRegistry mockRegistry = SimpleFacetGraphRepositoryRegistryFactory.CreateMockRegistry();
            return new RouteFinder(mockRegistry);
        }
    }
}
