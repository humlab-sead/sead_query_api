using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using NpgsqlTypes;

namespace SeadQueryInfra
{
    public static class DataReaderExtension
    {
        public static (T, T) GetRange<T>(this DbDataReader dr, int index)
        {
            NpgsqlRange<T> range = dr.GetFieldValue<NpgsqlRange<T>>(index);
            if (range.IsEmpty)
                return (default(T), default(T));
            return (range.LowerBound, range.UpperBound);
        }
    }

    public static class TypeLoaderExtensions
    {
        public static IEnumerable<Type> GetLoadableTypes(this Assembly assembly)
        {
            if (assembly == null) throw new ArgumentNullException(nameof(assembly));
            try
            {
                return assembly.GetTypes();
            }
            catch (ReflectionTypeLoadException e)
            {
                return e.Types.Where(t => t != null);
            }
        }

        public static IEnumerable<Type> GetTypesThatIsAssignableFrom<T>(this Assembly assembly)
        {
            return assembly
                .GetLoadableTypes()
                .Where(t => !(t.IsAbstract || t.IsInterface) && typeof(T).IsAssignableFrom(t))
                .ToList();
        }
    }
}
