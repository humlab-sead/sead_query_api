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

        private static FacetContext CreateContext()
        {
            var builder = ScaffoldUtility.GetDbContextOptionBuilder();
            return new FacetContext(builder.Options);
        }

        /// <summary>
        /// Serializes a new JSON facet model fixture to from data in online database
        /// </summary>
        /// <param name="updateTheModel">Set to true if you want to update fixture</param>
        [Theory]
        [InlineData(true)]
        public void UpdateFacetContextFixture_IfParameterIsSetToTrue(bool updateTheModel)
        {
            if (!updateTheModel)
                return;

            var serializer = CreateSerializer();
            var path = ScaffoldUtility.JsonDataFolder();

            using (var context = CreateContext()) {
                new JsonWriterService(serializer).SerializeTypesToPath(context, ScaffoldUtility.GetModelTypes(), path);
            }
        }
    }
}
