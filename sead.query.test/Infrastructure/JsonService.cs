using SeadQueryTest.Infrastructure;
using System;
using System.Collections.Generic;
using System.IO;

namespace SeadQueryTest.Fixtures
{
    public class JsonService
    {
        public static string DataFolder()
        {
            return Path.Combine(ScaffoldUtility.GetRootFolder(), "Infrastructure", "Data", "Json");
        }

        public static List<T> LoadJSON<T>() where T : class, new()
        {
            string folder = DataFolder();
            var reader = new JsonReaderService();
            List<T> entities = new List<T>(reader.Deserialize<T>(folder));
            return entities;
        }

        public static T LoadSingleJSON<T>(int index = 0) where T : class, new()
        {
            return LoadJSON<T>()[index];
        }

        protected System.Reflection.MethodInfo GetGenericMethodForType<T>(string name, Type type)
        {
            return typeof(T).GetMethod(name).MakeGenericMethod(new[] { type });
        }

    }
}
