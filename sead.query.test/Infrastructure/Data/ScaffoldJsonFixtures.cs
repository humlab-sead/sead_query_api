using Newtonsoft.Json;
using Xunit;
using System.IO;
using Newtonsoft.Json.Serialization;
using SeadQueryAPI.Serializers;
using SeadQueryCore;
using SeadQueryTest.Fixtures;
using SeadQueryTest.Mocks;
using SeadQueryInfra.DataAccessProvider;

namespace SeadQueryTest.Infrastructure.Scaffolding
{
    public class ScaffoldJsonFixtures
    {
        private class FixtureContractResolver : PropertyRenameAndIgnoreSerializerContractResolver
        {
            public FixtureContractResolver() : base()
            {
                IgnoreProperty(typeof(TableRelation),
                    "SourceTable",
                    "TargetTable",
                    "Key"
                );
            }
        }

        private static DefaultContractResolver DefaultResolver()
        {
            var resolver = new SeadQueryAPI.Serializers.SeadQueryResolver();
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

        [Fact(Skip = "Not a test. Scaffolds JSON data from online facet model")]
        public void ScaffoldFacetDatabaseModelToJsonFileUsingOnlineDatabase()
        {
            var serializer = CreateSerializer();
            var path = JsonService.DataFolder();
            using (var context = CreateContext()) {
                new JsonWriterService(serializer).SerializeTypesToPath(context, ScaffoldUtility.GetModelTypes(), path);
            }
        }

        [Fact]
        public void CanLoadScaffoldedJsonModelAsDbContext()
        {
            JsonSerializer serializer = new JsonSerializer
            {
                // serializer.Converters.Add(new JavaScriptDateTimeConverter());
                NullValueHandling = NullValueHandling.Ignore
            };

            var reader = new JsonReaderService();
            var path = JsonService.DataFolder();
            using (var context = CreateContext()) {
                var types = ScaffoldUtility.GetModelTypes();
                var facets = reader.Deserialize<SeadQueryCore.Facet>(path);
            }
        }

    }
}
