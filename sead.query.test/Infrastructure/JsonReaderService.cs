using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using SeadQueryAPI.Serializers;
using SeadQueryCore;
using SeadQueryInfra;
using SeadQueryTest.Fixtures;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SeadQueryTest.Infrastructure
{
    //public class JsonFileContractResolver : PropertyRenameAndIgnoreSerializerContractResolver
    //{
    //    public JsonFileContractResolver() : base()
    //    {
    //        IgnoreProperty(typeof(TableRelation),
    //            "SourceTable",
    //            "TargetTable"
    //        );
    //        IgnoreProperty(typeof(ResultAggregateField),
    //            "Aggregate"
    //        );
    //    }
    //}

    //public class FacetConverter : CustomCreationConverter<Facet>
    //{
    //    public FacetConverter(FacetContext context)
    //    {
    //        Context = context;
    //    }

    //    public FacetContext Context { get; }

    //    public override Facet Create(Type objectType)
    //    {
    //        return Context.Set<Facet>().;
    //    }
    //}

    public class JsonReaderService
    {
        public IContractResolver ContractResolver { get; }

        public JsonReaderService(IContractResolver contractResolver)
        {
            ContractResolver = contractResolver; // ?? new JsonFileContractResolver();
        }

        public IEnumerable<T> Deserialize<T>(string folder) where T : class, new()
        {
            var filename = Path.Combine(folder, $"{typeof(T).Name}.json");
            if (!File.Exists(filename)) {
                throw new FileNotFoundException(filename);
                // return new List<T>();
            }
            var settings = new JsonSerializerSettings()
            {
                ContractResolver = ContractResolver
            };
            using (StreamReader stream = new StreamReader(filename)) {
                var json = stream.ReadToEnd();
                IEnumerable<T> items = JsonConvert.DeserializeObject<IEnumerable<T>>(json, settings);
                return items;
            }
        }

        public IEnumerable<object> Deserialize(Type type, string folder)
        {
            return (IEnumerable<object>)typeof(JsonReaderService)
                .GetMethod("Deserialize")
                .MakeGenericMethod(type)
                .Invoke(this, new object[] { folder });
        }

    }
}
