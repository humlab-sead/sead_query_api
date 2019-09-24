using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace sead.query.test.data.Infrastructure
{
    public class ScaffoldService
    {
        //protected System.Reflection.MethodInfo GetDbContextSetMethodForType(Type entityType)
        //{
        //    // Get the generic method `Set` and Make the non-generic method via the `MakeGenericMethod` reflection call.
        //    return typeof(DbContext).GetMethod("Set").MakeGenericMethod(new[] { entityType });
        //}

        protected System.Reflection.MethodInfo GetGenericMethodForType<T>(string name, Type type)
        {
            return typeof(T).GetMethod(name).MakeGenericMethod(new[] { type });
        }

    }

    public class ScaffoldWriter : ScaffoldService
    {
        public ScaffoldWriter(JsonSerializer serializer)
        {
            Serializer = serializer;
        }

        public JsonSerializer Serializer { get; }

        public void SerializeTypesToPath(DbContext context, ICollection<Type> types, string path)
        {
            foreach (var type in types) {
                object entities = GetEntititesForType(context, type);
                SerializeToFile(type, entities, path);
            }
        }

        public void SerializeToFile(Type type, object entities, string path)
        {
            string filename = Path.Combine(path, $"{type.Name}.json");
            using (StreamWriter sw = new StreamWriter(filename))
            using (JsonWriter writer = new JsonTextWriter(sw)) {
                Serializer.Serialize(writer, entities);
            }
        }
        public void SerializeToFile<T>(object entities, string path)
        {
            SerializeToFile(typeof(T), entities, path);
        }

        private object GetEntititesForType(DbContext context, Type type)
        {
            return GetGenericMethodForType<DbContext>("Set", type).Invoke(context, new object[] { });
        }
    }
}
