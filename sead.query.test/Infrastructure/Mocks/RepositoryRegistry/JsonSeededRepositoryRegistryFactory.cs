using Moq;
using SeadQueryCore;
using SeadQueryInfra;
using SeadQueryTest.Fixtures;
using System.Data.Common;

namespace SeadQueryTest.Mocks
{
    internal static class JsonSeededRepositoryRegistryFactory
    {
        public static RepositoryRegistry Create(DbConnection connection = null)
        {
            var context = JsonSeededFacetContextFactory.Create(connection);
            var registry = new SeadQueryInfra.RepositoryRegistry(context);

            return registry;
        }
    }
}
