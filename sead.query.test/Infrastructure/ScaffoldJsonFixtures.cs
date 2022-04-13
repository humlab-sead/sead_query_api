using Newtonsoft.Json;
using Xunit;
using System.IO;
using Newtonsoft.Json.Serialization;
using SQT.Fixtures;
using SQT.Mocks;
using SeadQueryInfra;
using SQT.Infrastructure;

namespace SQT.TestInfrastructure
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

        private static FacetContext CreateContext(string hostName, string databaseName)
        {
            var builder = ScaffoldUtility.GetDbContextOptionBuilder(hostName, databaseName);
            return new FacetContext(builder.Options);
        }

        /// <summary>
        /// Serializes a new JSON facet model fixture to from data in online database
        /// </summary>
        /// <param name="updateTheModel">Set to true if you want to update fixture</param>
        [Theory]
        //[InlineData(true, "127.0.0.1", "fitness", "Infrastructure/Data/fitness/Json")]
        [InlineData(false, "127.0.0.1", "sead_staging", "Infrastructure/Data/Json")]
        public void UpdateFacetContextFixture_IfParameterIsSetToTrue(bool updateTheModel, string hostName, string databaseName, string folder)
        {
            if (!updateTheModel)
                return;

            var serializer = CreateSerializer();
            var path = Path.Combine(ScaffoldUtility.GetRootFolder(), folder);

            using (var context = CreateContext(hostName, databaseName)) {
                new JsonWriterService(serializer).SerializeTypesToPath(context, ScaffoldUtility.GetModelTypes(), path);
            }
        }
    }
}
