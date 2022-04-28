using System;
using System.Collections.Generic;
using System.IO;
using Xunit;

namespace SQT.Infrastructure
{
    public class SeadJsonFacetContextFixture : JsonFacetContextFixture
    {
        public SeadJsonFacetContextFixture() : base(Path.Combine(ScaffoldUtility.GetRootFolder(), "Infrastructure", "Data", "Json"))
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
