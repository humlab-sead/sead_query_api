using DataAccessPostgreSqlProvider;
using KellermanSoftware.CompareNetObjects;
using Microsoft.EntityFrameworkCore;
using SeadQueryTest.Infrastructure.Scaffolding;
using System;
using System.Collections.Generic;
using System.Reflection;
using Xunit;

namespace SeadQueryTest.Infrastructure
{
    public class FacetTestBase
    {

        public static class Asserter
        {
            public static bool EqualByProperty(object object1, object object2)
            {
                CompareLogic compareLogic = new CompareLogic();
                ComparisonResult result = compareLogic.Compare(object1, object2);
               
                return result.AreEqual;
            }

            public static bool EqualByDictionary(Type type, Dictionary<string, object> expected, object entity)
            {
                foreach (KeyValuePair<string, object> entry in expected) {
                    PropertyInfo targetProperty = type.GetProperty(entry.Key, BindingFlags.Public | BindingFlags.Instance);
                    Assert.NotNull(targetProperty);
                    Assert.Equal(entry.Value, targetProperty.GetValue(entity));
                }
                return true;
            }

            public static void EqualByDictionary(Dictionary<string, object> expected, object entity)
            {
                EqualByDictionary(entity.GetType(), expected, entity);
            }

        }

        protected MethodInfo GetGenericMethodForType<T>(string name, Type type)
        {
            return typeof(T).GetMethod(name).MakeGenericMethod(new[] { type });
        }

    }
}
