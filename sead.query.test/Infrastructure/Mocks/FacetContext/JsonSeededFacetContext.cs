using Microsoft.EntityFrameworkCore;
using SeadQueryInfra;
using SeadQueryTest.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SeadQueryTest.Mocks
{
    public class JsonSeededFacetContext : FacetContext
    {
        public string Folder { get; }

        public JsonSeededFacetContext(DbContextOptions options, string folder) : this(options)
        {
            Folder = folder;
        }

        public JsonSeededFacetContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            var reader = new JsonReaderService(new IgnoreJsonAttributesResolver());
            foreach (var type in ScaffoldUtility.GetModelTypes()) {
                builder.Entity(type).HasData(
                    Deserialize(type, reader).ToArray()
                );
            }
        }

        public IEnumerable<object> Deserialize(Type type, JsonReaderService reader)
        {
            return (IEnumerable<object>)typeof(JsonReaderService)
                .GetMethod("Deserialize")
                .MakeGenericMethod(type)
                .Invoke(reader, new object[] { Folder });
        }
    }
}
