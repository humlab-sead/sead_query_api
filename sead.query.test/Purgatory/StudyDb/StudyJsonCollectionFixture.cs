using System.IO;
using SQT.Infrastructure;
using SQT.Scaffolding;
using Xunit;

namespace Deprecated.StudyDb
{
    public class StudyJsonFacetContextFixture : JsonFacetContextDataFixture
    {
        public StudyJsonFacetContextFixture() :
            base("Data/StudyDb")
        {
        }
    }

    [CollectionDefinition("StudyJsonSeededFacetContext")]
    public class StudyJsonCollectionFixture : ICollectionFixture<StudyJsonFacetContextFixture>
    {
        // This class has no code, and is never created. Its purpose is simply
        // to be the place to apply [CollectionDefinition] and all the
        // ICollectionFixture<> interfaces.
    }
}
