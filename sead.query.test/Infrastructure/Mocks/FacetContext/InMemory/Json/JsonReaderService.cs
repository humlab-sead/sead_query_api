﻿using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace SQT.Infrastructure
{


    public class JsonReaderService
    {
        public IContractResolver ContractResolver { get; }
        public MethodInfo DeserializeMethodInfo { get; }

        public JsonReaderService(IContractResolver contractResolver)
        {
            ContractResolver = contractResolver; // ?? new JsonFileContractResolver();
            DeserializeMethodInfo = typeof(JsonReaderService)
                .GetMethods().Single(x => x.Name == "Deserialize" && x.IsGenericMethodDefinition);
        }

        public IEnumerable<T> Deserialize<T>(string folder) where T : class, new()
        {
            var filename = Path.Combine(folder, $"{typeof(T).Name}.json");
            if (!File.Exists(filename))
            {
                throw new FileNotFoundException(filename);
                // return new List<T>();
            }
            var settings = new JsonSerializerSettings()
            {
                ContractResolver = ContractResolver
            };
            // var json = JsonTextLoader.Get(filename);
            using (StreamReader stream = new StreamReader(filename))
            {
                var json = stream.ReadToEnd();
                IEnumerable<T> items = JsonConvert.DeserializeObject<IEnumerable<T>>(json, settings);
                return items;
            }
        }

        public IEnumerable<object> Deserialize(Type type, string folder)
        {
            return (IEnumerable<object>)DeserializeMethodInfo
                .MakeGenericMethod(type)
                .Invoke(this, new object[] { folder });
        }
    }
}
