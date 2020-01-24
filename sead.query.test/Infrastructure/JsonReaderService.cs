using Newtonsoft.Json;
using SeadQueryAPI.Serializers;
using SeadQueryCore;
using SeadQueryTest.Fixtures;
using System.Collections.Generic;
using System.IO;

namespace SeadQueryTest.Infrastructure
{
    public class JsonReaderService : JsonService
    {
        private class JsonFileContractResolver : PropertyRenameAndIgnoreSerializerContractResolver
        {
            public JsonFileContractResolver() : base()
            {
                IgnoreProperty(typeof(TableRelation),
                    "SourceTable",
                    "TargetTable"
                );
                IgnoreProperty(typeof(ResultAggregateField),
                    "Aggregate"
                );
            }
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
                ContractResolver = new JsonFileContractResolver()
            };
            using (StreamReader stream = new StreamReader(filename)) {
                var json = stream.ReadToEnd();
                IEnumerable<T> items = JsonConvert.DeserializeObject<IEnumerable<T>>(json, settings);
                return items;
            }
        }

        //private static List<T> DeserializeItems<T>(string json) where T : class, new()
        //{
        //    dynamic d_data = JsonConvert.DeserializeObject<dynamic>(json);
        //    dynamic d_items = d_data.data;
        //    List<T> items = new List<T>();
        //    Type targetType = typeof(T);
        //    foreach (dynamic d_item in d_items) {
        //        T item = new T();
        //        foreach (JProperty sourceProperty in d_item.Properties()) {
        //            PropertyInfo targetProperty = targetType.GetProperty(sourceProperty.Name, BindingFlags.Public | BindingFlags.Instance);
        //            if (targetProperty != null && targetProperty.CanWrite) {
        //                var value = Convert.ChangeType(sourceProperty.Value, targetProperty.PropertyType);
        //                targetProperty.SetValue(item, value, null);
        //            }
        //        }
        //        items.Add(item);
        //    }

        //    return items;
        //}

        //public ICollection<T> Deserialize2<T>(string folder) where T : class, new()
        //{
        //    var filename = Path.Combine(folder, $"{typeof(T).Name}.json");
        //    if (!File.Exists(filename)) {
        //        return new List<T>();
        //    }
        //    using (StreamReader stream = new StreamReader(filename)) {
        //        var json = stream.ReadToEnd();
        //        var items = JsonConvert.DeserializeObject<List<T>>(json);
        //        return items;
        //    }
        //}

        //public IEnumerable<object> CreateList(Type type)
        //{
        //    Type genericListType = typeof(List<>).MakeGenericType(type);
        //    return (IEnumerable<object>)Activator.CreateInstance(genericListType);
        //}

    }

}
