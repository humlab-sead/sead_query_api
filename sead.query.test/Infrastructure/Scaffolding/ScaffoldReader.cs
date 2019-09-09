using AutoMapper;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using SeadQueryCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;

namespace SeadQueryTest.Infrastructure.Scaffolding
{
    public class ScaffoldReader : ScaffoldService
    {
        public ICollection<T> DeserializePPP<T>(string folder) where T : class, new()
        {

            var filename = Path.Combine(folder, $"{typeof(T).Name}.json");
            if (!File.Exists(filename)) {
                throw new FileNotFoundException(filename);
                return new List<T>();
            }
            using (StreamReader stream = new StreamReader(filename)) {
                var json = $"{{ \"data\": {stream.ReadToEnd()} }}";
                dynamic d_data = JsonConvert.DeserializeObject<dynamic>(json);
                dynamic d_items = d_data.data;
                List<T> items = new List<T>();
                Type targetType = typeof(T);
                foreach (dynamic d_item in d_items) {
                    T item = new T();
                    foreach (JProperty sourceProperty in d_item.Properties()) {
                        PropertyInfo targetProperty = targetType.GetProperty(sourceProperty.Name, BindingFlags.Public | BindingFlags.Instance);
                        if (targetProperty != null && targetProperty.CanWrite) {
                            var value = Convert.ChangeType(sourceProperty.Value, targetProperty.PropertyType);
                            targetProperty.SetValue(item, value, null);
                        }
                    }
                    items.Add(item);
                }
                return items;
            }
        }

        #region __Catastrophically_Failed_Attempts_To_Do_A_Simple_Deserialization__
        public ICollection<T> Deserialize<T>(string folder)
        {
            var filename = Path.Combine(folder, $"{typeof(T).Name}.json");
            if (!File.Exists(filename)) {
                throw new FileNotFoundException(filename);
                return new List<T>();
            }

            var settings = new JsonSerializerSettings() {
                ContractResolver = new IncludeIgnoreJsonPropertiesResolver()
            };
            using (StreamReader stream = new StreamReader(filename)) {
                var json = stream.ReadToEnd();
                var items = JsonConvert.DeserializeObject<List<T>>(json, settings);
                return items;
            }
        }

        //public IEnumerable<object> createList(Type type)
        //{
        //    Type genericListType = typeof(List<>).MakeGenericType(type);
        //    return (IEnumerable<object>)Activator.CreateInstance(genericListType);
        //}
        public class TestDTO<T> {
            public ICollection<T> data { get; set;  }
        }
        public ICollection<T> DeserializeD<T>(string folder) where T : class, new()
        {
            var filename = Path.Combine(folder, $"{typeof(T).Name}.json");
            if (!File.Exists(filename)) {
                throw new FileNotFoundException(filename);
                return new List<T>();
            }

            using (StreamReader stream = new StreamReader(filename)) {
                var json = $"{{ \"data\": {stream.ReadToEnd()} }}";
                // dynamic d_items = JObject.Parse(json);
                TestDTO<T> d_data = JsonConvert.DeserializeObject<TestDTO<T>>(json);
                return d_data.data;
            }
        }

        public ICollection<T> DeserializeG<T>(string folder) where T : class, new()
        {
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<JValue, object>().ConvertUsing(source => source.Value);
                cfg.CreateMap<long, int>().ConvertUsing(d => (int)(d));
                cfg.CreateMap<long, EFacetType>().ConvertUsing(d => (EFacetType)(d));
            });
            //configuration.AssertConfigurationIsValid();

            var mapper = configuration.CreateMapper();

            var filename = Path.Combine(folder, $"{typeof(T).Name}.json");
            if (!File.Exists(filename)) {
                throw new FileNotFoundException(filename);
                return new List<T>();
            }

            using (StreamReader stream = new StreamReader(filename)) {
                var json = $"{{ \"data\": {stream.ReadToEnd()} }}";
                // dynamic d_items = JObject.Parse(json);
                dynamic d_data = JsonConvert.DeserializeObject<dynamic>(json);
                dynamic d_items = d_data.data;
                List<T> items = new List<T>();
                foreach (dynamic d_item in d_items) {
                    var item = mapper.Map<T>(d_item);
                    items.Add(item);
                }
                return items;
            }
        }
        #endregion
    }

    #region __Deprecated_Stuff__

    public class StartsWithContractResolver : DefaultContractResolver
    {
        private readonly char _startingWithChar;
        public StartsWithContractResolver(char startingWithChar)
        {
            _startingWithChar = startingWithChar;
        }

        protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
        {
            IList<JsonProperty> properties = base.CreateProperties(type, memberSerialization);

            // only serializer properties that start with the specified character
            properties =
              properties.Where(p => p.PropertyName.StartsWith(_startingWithChar.ToString())).ToList();

            return properties;
        }
    }

    public class SkipKeysContractResolver : DefaultContractResolver
    {
        private IList<string> attributesList = null;
        public SkipKeysContractResolver(IList<string> propertiesToSerialize)
        {
            attributesList = propertiesToSerialize;
        }

        protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
        {
            IList<JsonProperty> properties = base.CreateProperties(type, memberSerialization);
            return properties.Where(p => attributesList.Contains(p.PropertyName)).ToList();
        }
    }
    class IncludeIgnoreJsonPropertiesResolver : DefaultContractResolver
    {
        protected List<MemberInfo> skippedProps = new List<MemberInfo>();
        protected override List<MemberInfo> GetSerializableMembers(Type objectType)
        {
            var props = base.GetSerializableMembers(objectType).ToList<MemberInfo>();
            skippedProps = objectType
                .GetProperties()
                .Where(pi => Attribute.IsDefined(pi, typeof(JsonIgnoreAttribute)))
                .ToList<MemberInfo>();

            props.AddRange(skippedProps.FindAll(z => !props.Contains<MemberInfo>(z)));
            return props;
        }

        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            JsonProperty property = base.CreateProperty(member, memberSerialization);
            if (skippedProps.Any(z => z.Name == property.PropertyName)) {
                property.Ignored = false;
                property.ShouldSerialize = _ => true;
                property.ShouldDeserialize = _ => true;
            }
            return property;
        }
    }

    public class IgnoreAnnotationsContractResolver : DefaultContractResolver
    {
        protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
        {
            IList<JsonProperty> properties = base.CreateProperties(type, memberSerialization);
            return properties;
        }

        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            JsonProperty property = base.CreateProperty(member, memberSerialization);

            //if (member.MemberType.ToString() == "Property" && member.Name == "FacetTypeId") {
            //    foreach (var attribute in member.CustomAttributes) {
            //        if (attribute.AttributeType.Name == "JsonIgnoreAttribute") {
            //            property.ShouldSerialize = instance => { return true; };
            //            Console.WriteLine("HALT");
            //        }
            //    }
            //}

            return property;
        }

        //protected override List<MemberInfo> GetSerializableMembers(Type objectType)
        //{
        //    return base.GetSerializableMembers(objectType);
        //}

        // LowerCase example
        // protected override string ResolvePropertyName(string key)
        //{
        //    return key.ToLower();
        //}

        protected override List<MemberInfo> GetSerializableMembers(Type objectType)
        {
            IEnumerable<MemberInfo> allMembers = GetFieldsAndProperties(objectType, BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);
            List<MemberInfo> serializableMembers = new List<MemberInfo>();
            foreach (MemberInfo member in allMembers) {
                if (member.IsDefined(typeof(CompilerGeneratedAttribute), true))
                    continue;
                if (member is FieldInfo field) {
                    if (field.IsStatic)
                        continue;
                }
                if (member.MemberType != MemberTypes.Property)
                    continue;
                serializableMembers.Add(member);
            }
            return serializableMembers;
        }

        public static List<MemberInfo> GetFieldsAndProperties(Type type, BindingFlags bindingAttr)
        {
            List<MemberInfo> targetMembers = new List<MemberInfo>();

            targetMembers.AddRange(GetFields(type, bindingAttr));
            targetMembers.AddRange(GetProperties(type, bindingAttr));

            return targetMembers;
        }

        public static IEnumerable<FieldInfo> GetFields(Type targetType, BindingFlags bindingAttr)
        {
            List<MemberInfo> fieldInfos = new List<MemberInfo>(targetType.GetFields(bindingAttr));
            return fieldInfos.Cast<FieldInfo>();
        }

        public static IEnumerable<PropertyInfo> GetProperties(Type targetType, BindingFlags bindingAttr)
        {
            List<PropertyInfo> propertyInfos = new List<PropertyInfo>(targetType.GetProperties(bindingAttr));
            return propertyInfos;
        }


    }

    #endregion
}
