﻿using System.IO;
using SQT.Scaffolding;
using Xunit;

namespace SQT.Infrastructure
{
    public class StudyJsonFacetContextFixture : JsonFacetContextDataFixture
    {
        public StudyJsonFacetContextFixture() :
            base(Path.Combine(ScaffoldUtility.GetRootFolder(), "Infrastructure", "Data", "StudyDb", "Json"))
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
