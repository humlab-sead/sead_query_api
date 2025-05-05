using AutoFixture;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using SeadQueryCore;
using SeadQueryInfra;
using SQT.Infrastructure;
using SQT.Scaffolding;
using System.Threading.Tasks;

namespace SQT.Mocks;

public class JsonSeededFacetContextFactory
{
    public virtual async Task<FacetContext> CreateAsync(DbContextOptions options, string jsonFolder) //, string modelSchemaFilename = null)
    {
        var fixture = new JsonFacetContextDataFixture(ScaffoldUtility.GetDataFolder(jsonFolder));
        using (var context = new JsonSeededFacetContext(options, fixture))
        {
            await context.Database.EnsureCreatedAsync();
            await context.SaveChangesAsync();
            return context;
        }
    }

    public virtual FacetContext Create(DbContextOptions options, string jsonFolder) //, string modelSchemaFilename = null)
    {
        return CreateAsync(options, jsonFolder).GetAwaiter().GetResult();
    }

    public virtual FacetContext CreateSqlite(string jsonFolder)
    {
        using (var connection = new SqliteConnection("DataSource=:memory:;Foreign Keys = False"))
        {
            connection.Open();
            var options = new DbContextOptionsBuilder<FacetContext>().UseSqlite(connection).Options;
            return Create(options, jsonFolder);
        }
    }
}
