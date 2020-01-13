using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Xunit;
using DataAccessPostgreSqlProvider;
using System.IO;
using Newtonsoft.Json.Serialization;
using SeadQueryAPI.Serializers;
using SeadQueryCore;

namespace SeadQueryTest.Infrastructure.Scaffolding
{
    public class ScaffoldJsonFixtures
    {
        public class FixtureContractResolver : PropertyRenameAndIgnoreSerializerContractResolver
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

        private string TargetFolder()
        {
            return Path.Combine(ScaffoldUtility.GetRootFolder(), "Infrastructure", "JsonFixtures", "Data");
        }

        private static ScaffoldContext CreateContext()
        {
            var builder = ScaffoldUtility.GetDbContextOptionBuilder();
            return new ScaffoldContext(builder.Options); // FacetContext
        }

        [Fact(Skip = "Not a test. Scaffolds JSON data from online facet model")]
        public void ScaffoldFacetDatabaseModelToJsonFileUsingOnlineDatabase()
        {
            var serializer = CreateSerializer();
            var path = TargetFolder();
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
            var path = TargetFolder();
            using (var context = CreateContext()) {
                var types = ScaffoldUtility.GetModelTypes();
                var facets = reader.Deserialize<SeadQueryCore.Facet>(path);
            }
        }

    }
}
