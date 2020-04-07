using Microsoft.EntityFrameworkCore;
using Moq;
using SeadQueryCore;
using SeadQueryInfra;
using SeadQueryTest.Fixtures;
using SeadQueryTest.Infrastructure;
using System.Data.Common;

namespace SeadQueryTest.Mocks
{
    internal static class JsonSeededRepositoryRegistryFactory
    {
        public static RepositoryRegistry Create(DbContextOptions options, JsonSeededFacetContextFixture fixture)
        {
            var context = JsonSeededFacetContextFactory.Create(options, fixture);
            var registry = new SeadQueryInfra.RepositoryRegistry(context);

            return registry;
        }
    }
}
