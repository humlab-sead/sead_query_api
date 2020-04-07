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
    //public static class JsonTextLoader
    //{

    //    public static object lockObject = new object();
    //    public static Dictionary<string, string> __Cache = new Dictionary<string, string>();

    //    public static string Get(string filename)
    //    {
    //        if (!__Cache.ContainsKey(filename)) {
    //            lock (lockObject) {
    //                if (!__Cache.ContainsKey(filename)) {
    //                    using (StreamReader stream = new StreamReader(filename)) {
    //                        var json = stream.ReadToEnd();
    //                        __Cache.Add(filename, json);
    //                    }
    //                }
    //            }
    //        }
    //        return __Cache[filename];
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
            // var json = JsonTextLoader.Get(filename);
            using (StreamReader stream = new StreamReader(filename)) {
                var json = stream.ReadToEnd();
                IEnumerable<T> items = JsonConvert.DeserializeObject<IEnumerable<T>>(json, settings);
                return items;
            }
        }

        public IEnumerable<object> Deserialize(Type type, string folder)
        {
            return (IEnumerable<object>)typeof(JsonReaderService)
                .GetMethods().Single(x => x.Name == "Deserialize" && x.IsGenericMethodDefinition)
                .MakeGenericMethod(type)
                .Invoke(this, new object[] { folder });
        }

    }
}
