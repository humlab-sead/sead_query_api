using Microsoft.EntityFrameworkCore;
using Moq;
using SeadQueryCore;
using SeadQueryInfra;
using SQT.Fixtures;
using SQT.Infrastructure;
using System.Data.Common;

namespace SQT.Mocks
{
    internal static class JsonSeededRepositoryRegistryFactory
    {
        public static RepositoryRegistry Create(DbContextOptions options, JsonFacetContextFixture fixture)
        {
            var context = JsonSeededFacetContextFactory.Create(options, fixture);
            var registry = new SeadQueryInfra.RepositoryRegistry(context);

            return registry;
        }
    }
}
