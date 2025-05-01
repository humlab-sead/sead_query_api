using SQT.Scaffolding;
using Xunit;

namespace SQT.Infrastructure
{
    public class SeadJsonFacetContextFixture : JsonFacetContextDataFixture
    {
        public SeadJsonFacetContextFixture() : base(ScaffoldUtility.GetDataFolder("Json"))
        {
        }
    }

    [CollectionDefinition("SeadJsonFacetContextFixture")]
    public class SeadJsonCollectionFixture : ICollectionFixture<SeadJsonFacetContextFixture>
    {
        // This class has no code, and is never created. Its purpose is simply
        // to be the place to apply [CollectionDefinition] and all the
        // ICollectionFixture<> interfaces.
    }
}
