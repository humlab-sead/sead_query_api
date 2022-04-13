using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace SeadQueryAPI.Serializers
{
    public class PropertyFilterResolver : DefaultContractResolver
    {
        private const string _Err = "A type can be either in the include list or the ignore list.";
        private readonly PropertyMap _IgnorePropertiesMap = new PropertyMap();
        private readonly PropertyMap _IncludePropertiesMap = new PropertyMap();
        private readonly Dictionary<string, bool> GlobalProperties = new Dictionary<string, bool>();

        /// <summary>
        /// This list overrides properties in GlobalIncludedProperties.
        /// </summary>
        public PropertyFilterResolver SetIgnoredProperties<T>(params Expression<Func<T, object>>[] propertyAccessors)
        {
            if (propertyAccessors == null) return this;

            if (_IncludePropertiesMap.ContainsKey(typeof(T))) throw new ArgumentException(_Err);

            _IgnorePropertiesMap[typeof(T)] = propertyAccessors.Select(GetPropertyName);
            return this;
        }

        /// <summary>
        /// This list overrides properties in GlobalIgnoredProperties.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="propertyAccessors"></param>
        /// <returns></returns>
        public PropertyFilterResolver SetIncludedProperties<T>(params Expression<Func<T, object>>[] propertyAccessors)
        {
            if (propertyAccessors == null) return this;

            if (_IgnorePropertiesMap.ContainsKey(typeof(T))) throw new ArgumentException(_Err);

            _IncludePropertiesMap[typeof(T)] = propertyAccessors.Select(GetPropertyName);
            return this;
        }

        public PropertyFilterResolver Ignore(string globalIgnore)
        {
            GlobalProperties[globalIgnore] = false;
            return this;
        }

        /// <summary>
        /// Will act as global black list for all types, excecpt for those in the property maps,
        /// via <see cref="SetIncludedProperties"/>
        /// or <see cref="SetIgnoredProperties"/>.
        /// </summary>
        public PropertyFilterResolver Include(string globalInclude)
        {
            GlobalProperties[globalInclude] = true;
            return this;
        }

        protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
        {
            var properties = base.CreateProperties(type, memberSerialization);

            IEnumerable<string> map;
            var isIgnoreList = (map = _IgnorePropertiesMap[type]) != null;
            if (isIgnoreList || (map = _IncludePropertiesMap[type]) != null)
            {
                var globals = GlobalProperties.Where(kv => kv.Value != isIgnoreList);
                map = map
                  .Concat(globals.Select(kv => kv.Key))
                  .Distinct();
                return properties
                  .Where(jp => map.Contains(jp.PropertyName) != isIgnoreList)
                  .ToArray();
            }
            else
            {
                return properties
                  .Where(p =>
                  {
                      var include = true;
                      var globalIncludes = GlobalProperties
                .Where(kv => kv.Value == include);
                      var globalProperties = globalIncludes
                .Select(kv => kv.Key);
                      if (globalIncludes.Any())
                      {
                          return globalProperties.Contains(p.PropertyName);
                      }
                      else
                      {
                          include = false;
                          return !p.Ignored && !globalProperties.Contains(p.PropertyName);
                      }
                  })
                  .ToArray();
            }
        }

        private string GetPropertyName<TSource, TProperty>(
        Expression<Func<TSource, TProperty>> propertyLambda)
        {
            var body = propertyLambda.Body;
            if (!(body is MemberExpression member)
               && !(body is UnaryExpression unary && (member = unary.Operand as MemberExpression) != null))
            {
                throw new ArgumentException($"Expression '{propertyLambda}' refers to a method, not a property.");
            }

            if (!(member.Member is PropertyInfo propInfo))
                throw new ArgumentException($"Expression '{propertyLambda}' refers to a field, not a property.");

            var type = typeof(TSource);
            if (!propInfo.DeclaringType.GetTypeInfo().IsAssignableFrom(type.GetTypeInfo()))
                throw new ArgumentException($"Expresion '{propertyLambda}' refers to a property that is not from type '{type}'.");

            return propInfo.Name;
        }

        private class PropertyMap
        {
            private readonly Dictionary<Type, HashSet<string>> Map = new Dictionary<Type, HashSet<string>>();

            public IEnumerable<string> this[Type type]
            {
                get
                {
                    if (type == null) throw new ArgumentNullException(nameof(type));
                    if (Map.TryGetValue(type, out HashSet<string> value))
                        return value;

                    var result = Map
                      .Where(kv => kv.Key.GetTypeInfo().IsAssignableFrom(type.GetTypeInfo()))
                      .SelectMany(kv => kv.Value ?? Enumerable.Empty<string>());

                    return Map[type] = result.Any() ? new HashSet<string>(result) : null;
                }
                set
                {
                    if (type == null) throw new ArgumentNullException(nameof(type));
                    if (value == null) throw new ArgumentNullException(nameof(value));

                    if (Map.TryGetValue(type, out var existing))
                    {
                        foreach (var property in value)
                            existing.Add(property);
                    }
                    else
                    {
                        Map[type] = new HashSet<string>(value);
                    }
                }
            }

            public bool ContainsKey(Type type) => Map.ContainsKey(type);
        }
    }
}
