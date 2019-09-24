using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace sead.query.test.data.Infrastructure
{
    public class ScaffoldReader : ScaffoldService
    {
        public ICollection<T> Deserialize<T>(string folder)
        {
            var filename = Path.Combine(folder, $"{typeof(T).Name}.json");
            if (!File.Exists(filename)) {
                return new List<T>();
            }
            using (StreamReader stream = new StreamReader(filename)) {
                var json = stream.ReadToEnd();
                var items = JsonConvert.DeserializeObject<List<T>>(json);
                return items;
            }
        }

        public IEnumerable<object> createList(Type type)
        {
            Type genericListType = typeof(List<>).MakeGenericType(type);
            return (IEnumerable<object>)Activator.CreateInstance(genericListType);
        }
    }
}
