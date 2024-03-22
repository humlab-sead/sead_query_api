using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SeadQueryInfra
{
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
