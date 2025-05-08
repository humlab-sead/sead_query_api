using Newtonsoft.Json;
using Xunit;
using System.IO;
using Newtonsoft.Json.Serialization;
using SeadQueryInfra;
using SQT.Infrastructure;
using SQT.Scaffolding.Json;
using SQT.Scaffolding;
namespace SQT.TestInfrastructure
{
    public class GenerateJson
    {
        private static DefaultContractResolver DefaultResolver()
        {
            var resolver = new IgnoreJsonAttributesResolver();
            return resolver;
        }

        private static JsonSerializer CreateSerializer()
        {
            var serializer = new JsonSerializer
            {
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
            var builder = Utility.GetDbContextOptionBuilder(hostName, databaseName);
            return new FacetContext(builder.Options);
        }

        /// <summary>
        /// Serializes a new JSON facet model fixture to from data in online database
        /// </summary>
        /// <param name="updateTheModel">Set to true if you want to update fixture</param>
        [Theory]
        [InlineData(false, "Infrastructure/Data/Json")]
        public void UpdateFacetContextFixture_IfParameterIsSetToTrue(bool updateTheModel, string folder)
        {
            if (!updateTheModel){
                Debug.WriteLine("Skipping update of JSON fixtures");
                return;
            }
            var options = SettingFactory.DefaultSettings;
            var serializer = CreateSerializer();
            var path = Path.Combine(ScaffoldUtility.GetRootFolder(), folder);

            using (var context = CreateContext(options.Store.Host, options.Store.Database))
            {
                new JsonWriterService(serializer).SerializeTypesToPath(context, ScaffoldUtility.GetModelTypes(), path);
            }
        }
    }
}
