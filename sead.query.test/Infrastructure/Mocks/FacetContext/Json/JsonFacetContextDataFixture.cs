using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using SQT.Scaffolding;

namespace SQT.Infrastructure
{
    using ItemsDictionary = Dictionary<Type, IEnumerable<object>>;

    public class JsonFacetContextDataFixture : IDisposable
    {
        /// <summary>
        /// Reads Json Facet Schema entities and stores them in a dictionary
        /// </summary>
        private readonly Lazy<ItemsDictionary> LazyItems;
        public ItemsDictionary Items => LazyItems.Value;
        public string Folder { get; }
        public ICollection<Type> Types { get; }

        public JsonFacetContextDataFixture(string folder)
        {
            Folder = Path.Combine(ScaffoldUtility.GetDataFolder(), folder);
            Types = ScaffoldUtility.GetModelTypes();
            LazyItems = new Lazy<ItemsDictionary>(Load);
        }

        protected ItemsDictionary Load()
        {
            // ... initialize data in the test database ...
            Console.WriteLine("INFO: JsonSeededFacetContextFixture");
            var reader = new JsonReaderService(new IgnoreJsonAttributesResolver());
            var items = new ItemsDictionary();
            foreach (var type in Types)
            {
                var entities = reader.Deserialize(type, Folder).ToArray();
                items.Add(type, entities);
            }
            return items;
        }

        public void Dispose()
        {
            // ... clean up test data...
        }
    }
}
