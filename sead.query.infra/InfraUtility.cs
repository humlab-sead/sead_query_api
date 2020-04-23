using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SeadQueryInfra
{
    public static class InfraUtility
    {
        //public static IEnumerable<Type> GetTypesThatIsAssignableFrom<T>(string assemblyNameStartsWith)
        //{
        //    return GetAssemblies(assemblyNameStartsWith)
        //        .SelectMany(a => a.GetTypesThatIsAssignableFrom<T>());
        //}

        public static IEnumerable<Type> GetTypesThatIsAssignableFrom<T>(string assemblyName)
        {
            return GetAssembly(assemblyName).GetTypesThatIsAssignableFrom<T>();
        }

        //public static IEnumerable<Assembly> GetAssemblies(string assemblyNameStartsWith)
        //    => AppDomain.CurrentDomain.GetAssemblies()
        //        .Where(a => a.GetName().Name.StartsWith(assemblyNameStartsWith));

        public static Assembly GetAssembly(string assemblyName)
            => AppDomain.CurrentDomain
                .GetAssemblies()
                .Where(a => a.GetName().Name.Equals(assemblyName))
                .FirstOrDefault();

        //public static Assembly GetAssembly2(string assemblyName)
        //     => Assembly.Load(assemblyName);

    }
}
