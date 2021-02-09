using Microsoft.EntityFrameworkCore;
using SeadQueryInfra;
using SQT.Infrastructure;
using System.Data.Common;
using System.Threading.Tasks;

namespace SQT.Mocks
{
    internal static class JsonSeededFacetContextFactory
    {
        public static FacetContext Create(DbContextOptions options, JsonFacetContextFixture fixture)
        {
            using (var context = new JsonSeededFacetContext(options, fixture)) {
                context.Database.EnsureCreated();
                context.SaveChanges();
            }
            return new FacetContext(options);
        }
    }
}
