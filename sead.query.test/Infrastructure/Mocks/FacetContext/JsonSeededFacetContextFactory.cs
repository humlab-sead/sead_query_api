using Microsoft.EntityFrameworkCore;
using SeadQueryInfra;
using SeadQueryTest.Infrastructure;
using System.Data.Common;
using System.Threading.Tasks;

namespace SeadQueryTest.Mocks
{
    internal static class JsonSeededFacetContextFactory
    {
        //public static async Task<FacetContext> Create(DbContextOptions options, string folder)
        public static FacetContext Create(DbContextOptions options, JsonSeededFacetContextFixture fixture)
        {
            using (var context = new JsonSeededFacetContext(options, fixture)) {
                context.Database.EnsureCreated();
                context.SaveChanges();
            }
            return new FacetContext(options);
        }

        //public static FacetContext Create(DbContextOptions options)
        //{
        //    return Create(options, ScaffoldUtility.JsonDataFolder());
        //}

        //public static FacetContext Create(DbConnection connection)
        //{
        //    var options = SqliteContextOptionsFactory.Create(connection);
        //    return Create(options, ScaffoldUtility.JsonDataFolder());
        //}

        //public static FacetContext Create()
        //{
        //    // FIXME: Refactor away, use using in test cases instead
        //    var connection = SqliteConnectionFactory.CreateAndOpen();
        //    return Create(connection);
        //}

    }
}
