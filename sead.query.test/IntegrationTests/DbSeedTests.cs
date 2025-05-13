using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SQT.Mocks;
using SQT.Scaffolding;
using Xunit;

namespace IntegrationTests.Deprecated
{
    public class SeedPublicTests
    {
        public SeedPublicTests() { }

        [Fact(Skip = "In-memory SQLite database is pending deprecation.")]
        public async Task TestMethod_UsingSqliteInMemoryProvider_Success()
        {
            var folder = ScaffoldUtility.GetPostgresDataFolder();
            using (var context = await new InMemoryFacetContextFactory().CreateAsync(folder, "facet"))
            {
                var count = await context.FacetGroups.CountAsync();
                Assert.True(count > 0);
                var u = await context.FacetGroups.FirstOrDefaultAsync(group => group.FacetGroupKey == "DOMAIN");
                Assert.NotNull(u);
                Assert.Equal(999, u.FacetGroupId);
            }
        }
    }
}
