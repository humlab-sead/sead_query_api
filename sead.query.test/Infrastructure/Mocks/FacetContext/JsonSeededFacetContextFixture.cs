using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace SeadQueryTest.Infrastructure
{
    public class JsonSeededFacetContextFixture : IDisposable
    {
        /// <summary>
        /// Reads Json Facet Schema entities and stores them in a dictionary
        /// </summary>
        public JsonSeededFacetContextFixture()
        {
            // ... initialize data in the test database ...
            var folder = ScaffoldUtility.JsonDataFolder();
            var reader = new JsonReaderService(new IgnoreJsonAttributesResolver());
            var items = new Dictionary<Type, IEnumerable<object>>();
            foreach (var type in ScaffoldUtility.GetModelTypes()) {
                var entities = reader.Deserialize(type, folder).ToArray();
                items.Add(type, entities);
            }
            Items = items;
        }

        public void Dispose()
        {
            // ... clean up test data...
        }

        public Dictionary<Type, IEnumerable<object>> Items { get; private set; }
    }

    [CollectionDefinition("Json seeded context")]
    public class JsonCollectionFixture : ICollectionFixture<JsonSeededFacetContextFixture>
    {
        // This class has no code, and is never created. Its purpose is simply
        // to be the place to apply [CollectionDefinition] and all the
        // ICollectionFixture<> interfaces.
    }
  
}
