using AutoFixture;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using SeadQueryCore;
using SeadQueryInfra;
using SQT.Infrastructure;
using SQT.Scaffolding;
using System.Data.Common;
using System.Threading.Tasks;

namespace SQT.Mocks;

public class JsonSeededFacetContextFactory
{
    public virtual async Task<FacetContext> CreateAsync(string jsonFolder)
    {
        var (context, _, _) = await CreateTupleAsync(jsonFolder);
        return context;
    }
    public virtual async Task<(InMemoryFacetContext, DbConnection, JsonFacetContextDataFixture)> CreateTupleAsync(string jsonFolder)
    {
        var fixture = new JsonFacetContextDataFixture(ScaffoldUtility.GetDataFolder(jsonFolder));
        var (options, connection) = await new SqliteConnectionFactory().CreateDbContextOptionsAsync2();
        var context = new InMemoryFacetContext(options, fixture, connection);
        await context.Database.EnsureCreatedAsync();
        await context.SaveChangesAsync();
        return (context, connection, fixture);
    }

    public virtual FacetContext Create(string jsonFolder)
    {
        return CreateAsync(jsonFolder).GetAwaiter().GetResult();
    }
}
