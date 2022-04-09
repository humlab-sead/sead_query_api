using Microsoft.EntityFrameworkCore;
using SeadQueryInfra;
using SQT.Infrastructure;
using System.Data.Common;
using System.Threading.Tasks;

namespace SQT.Mocks
{
    internal static class JsonSeededFacetContextFactory
    {
        public static FacetContext Create(DbContextOptions options, JsonFacetContextFixture fixture, string modelSchemaFilename=null)
        {
            var context = new JsonSeededFacetContext(options, fixture);
            context.ExecuteRawSqlFile(modelSchemaFilename);
            context.Database.EnsureCreated();
            context.SaveChanges();
            return context;
            // using (var context = new JsonSeededFacetContext(options, fixture)) {
            //     context.Database.EnsureCreated();
            //     context.SaveChanges();
            // }
            // // FIXME: why not return context????
            // return new FacetContext(options);
        }
    }
}
