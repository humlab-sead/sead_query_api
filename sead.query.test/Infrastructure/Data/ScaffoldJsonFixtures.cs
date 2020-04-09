using Newtonsoft.Json;
using Xunit;
using System.IO;
using Newtonsoft.Json.Serialization;
using SeadQueryTest.Fixtures;
using SeadQueryTest.Mocks;
using SeadQueryInfra;

namespace SeadQueryTest.Infrastructure.Scaffolding
{
    public class ScaffoldJsonFixtures
    {

        private static DefaultContractResolver DefaultResolver()
        {
            var resolver = new IgnoreJsonAttributesResolver();
            return resolver;
        }

        private static JsonSerializer CreateSerializer()
        {
            var serializer = new JsonSerializer {
                // serializer.Converters.Add(new JavaScriptDateTimeConverter());
                NullValueHandling = NullValueHandling.Ignore,
                Formatting = Formatting.Indented,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                ContractResolver = DefaultResolver()
            };
            return serializer;
        }

        private static FacetContext CreateContext()
        {
            var builder = ScaffoldUtility.GetDbContextOptionBuilder();
            return new FacetContext(builder.Options);
        }

        //[Fact(Skip = "Not a test. Scaffolds JSON data from")]
        //public void ScaffoldFacetConfigsToJsonFile()
        //{
        //    var serializer = CreateSerializer();
        //    var path = ScaffoldUtility.JsonDataFolder();
        //    using (var fixture = new JsonSeededFacetContextFixture())
        //    using (var container = new DisposableFacetContextContainer(fixture)) {
        //        var context = container.Context;

        //        new JsonWriterService(serializer).SerializeTypesToPath(container.Context, ScaffoldUtility.GetModelTypes(), path);
        //    }
        //}

        [Fact(Skip = "Not a test. Scaffolds JSON data from online facet model")]
        public void ScaffoldFacetDatabaseModelToJsonFileUsingOnlineDatabase()
        {
            var serializer = CreateSerializer();
            var path = ScaffoldUtility.JsonDataFolder();
            using (var context = CreateContext()) {
                new JsonWriterService(serializer).SerializeTypesToPath(context, ScaffoldUtility.GetModelTypes(), path);
            }
        }

        [Fact(Skip = "Not a test.")]
        public void CanLoadScaffoldedJsonModelAsDbContext()
        {
            JsonSerializer serializer = new JsonSerializer
            {
                // serializer.Converters.Add(new JavaScriptDateTimeConverter());
                NullValueHandling = NullValueHandling.Ignore
            };

            var reader = new JsonReaderService(DefaultResolver());
            var path = ScaffoldUtility.JsonDataFolder();
            using (var context = CreateContext()) {
                var types = ScaffoldUtility.GetModelTypes();
                var facets = reader.Deserialize<SeadQueryCore.Facet>(path);
            }
        }

    }
}
