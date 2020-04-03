using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using SeadQueryAPI.Serializers;
using SeadQueryCore;
using System.Collections.Generic;
using System;
using System.Diagnostics;

namespace SeadQueryTest.Infrastructure
{
        class IgnoreJsonAttributesResolver : PropertyRenameAndIgnoreSerializerContractResolver
    {
        private Dictionary<string, HashSet<string>> ignores = new Dictionary<string, HashSet<string>>();

        public IgnoreJsonAttributesResolver()
        {
            ignores = new Dictionary<string, HashSet<string>> {
                { typeof(Facet).FullName, new HashSet<string> { "FacetGroupKey", "FacetTypeKey" } }
            };

        }

        protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
        {
            IList<JsonProperty> props = base.CreateProperties(type, memberSerialization);
            foreach (var prop in props) {
                if (IsIgnored(prop)) {
                    prop.ShouldSerialize = obj => false;
                    prop.Ignored = true;
                    continue;
                }
                prop.Ignored = false;   // Ignore [JsonIgnore]
                prop.Converter = null;  // Ignore [JsonConverter]
                prop.PropertyName = prop.UnderlyingName;  // restore original property name
                //Debug.WriteLine($"{type.Name}.{prop.PropertyName}");
            }
            return props;
        }
        private bool IsIgnored(JsonProperty prop)
        {
            var type = prop.PropertyType;

            // Ignore all classes and interfaces...
            if ((type.IsClass || type.IsInterface) && type != typeof(string))
                return true;

            if (!prop.Writable)
                return true;

            if (!ignores.ContainsKey(type.Name)) {
                return false;
            }

            return ignores[type.Name].Contains(prop.PropertyName);
        }
    }
}
