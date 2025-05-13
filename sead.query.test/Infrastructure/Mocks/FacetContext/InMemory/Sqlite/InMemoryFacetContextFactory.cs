using System.Threading.Tasks;
using SQT.Scaffolding;

namespace SQT.Mocks;

public class InMemoryFacetContextFactory
{
    public virtual async Task<InMemoryFacetContext> CreateAsync(string folder, string schema)
    {
        var fixtureOptions = new InMemoryFacetContextOptions { Folder = folder, Schema = schema };
        var fixture = new CsvFacetContextDataFixture();
        var context = await InMemoryFacetContext.CreateAsync(fixtureOptions, fixture);
        await context.Database.EnsureCreatedAsync();
        await context.SaveChangesAsync();
        return context;
    }
}
