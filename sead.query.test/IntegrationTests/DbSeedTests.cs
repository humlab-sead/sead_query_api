using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using SeadQueryInfra;
using SQT.Infrastructure;
using SQT.Mocks;
using SQT.Scaffolding;
using Xunit;

namespace IntegrationTests.Deprecated
{
    public class SeedPublicTests
    {
        public SeedPublicTests()
        {
        }

        [Fact]
        public async Task TestMethod_UsingSqliteInMemoryProvider_Success()
        {
            using (var context = new JsonSeededFacetContextFactory().Create(ScaffoldUtility.GetInMemoryDataFolder("Data/FacetDb"))) 
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
