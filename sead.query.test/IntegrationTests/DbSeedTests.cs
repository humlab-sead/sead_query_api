using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using SeadQueryInfra;
using SQT.Infrastructure;
using SQT.Mocks;
using Xunit;

namespace IntegrationTests
{
    public class SeedPublicTests
    {
        public SeedPublicTests()
        {
        }

        [Fact]
        public async Task TestMethod_UsingSqliteInMemoryProvider_Success()
        {
            var folder = ScaffoldUtility.JsonDataFolder();
            var fixture = new SeadJsonFacetContextFixture();

            var modelSchemaFilename = Path.Join(GetDataFolder(), "sead_staging_sqlite_schema.sql");

            using (var connection = new SqliteConnection("DataSource=:memory:;Foreign Keys = False"))
            {
                connection.Open();

                var options = new DbContextOptionsBuilder<FacetContext>().UseSqlite(connection).Options;

                using (var context = JsonSeededFacetContextFactory.Create(options, fixture, modelSchemaFilename)) // new JsonSeededFacetContext(options, fixture))
                {
                    // context.Database.EnsureCreated();
                    // context.SaveChanges();
                    // context.ExecuteRawSqlFile(modelSchemaFilename);
                    // context.SaveChanges();

                    Console.WriteLine("TEST");
                }

                using (var context = Create(options, fixture))
                {
                    var count = await context.FacetGroups.CountAsync();
                    Assert.True(count > 0);
                    var u = await context.FacetGroups.FirstOrDefaultAsync(group => group.FacetGroupKey == "DOMAIN");
                    Assert.NotNull(u);
                    Assert.Equal(999, u.FacetGroupId);
                }
            }
        }

        private static string GetDataFolder() => Path.Combine(ScaffoldUtility.GetRootFolder(), "Infrastructure", "Data", "PublicModel");

        public static FacetContext Create(DbContextOptions options, JsonFacetContextDataFixture fixture)
        {
            using (var context = new JsonSeededFacetContext(options, fixture))
            {
                context.Database.EnsureCreated();
                context.SaveChanges();
            }
            return new FacetContext(options);
        }
    }
}
