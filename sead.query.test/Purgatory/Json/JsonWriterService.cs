using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SeadQueryCore;
using SeadQueryInfra;
using SQT.Fixtures;

namespace SQT.Infrastructure
{
    public class JsonWriterService
    {
        public JsonWriterService(JsonSerializer serializer)
        {
            Serializer = serializer;
        }

        public JsonSerializer Serializer { get; }

        public void SerializeTypesToPath(DbContext context, ICollection<Type> types, string path)
        {
            foreach (var type in types)
            {
                // This line prevents serialization to fail:
                var _ = context.Set<Facet>();
                object entities = context.SetEx(type);
                SerializeToFile(type, entities, path);
            }
        }

        public void SerializeToFile(Type type, object entities, string path)
        {
            string filename = Path.Combine(path, $"{type.Name}.json");
            using (StreamWriter sw = new StreamWriter(filename))
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                Serializer.Serialize(writer, entities);
            }
        }

        public void SerializeToFile<T>(object entities, string path)
        {
            SerializeToFile(typeof(T), entities, path);
        }
    }
}
