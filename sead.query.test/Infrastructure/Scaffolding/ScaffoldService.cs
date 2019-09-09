using System;

namespace SeadQueryTest.Infrastructure.Scaffolding
{
    public class ScaffoldService
    {
        protected System.Reflection.MethodInfo GetGenericMethodForType<T>(string name, Type type)
        {
            return typeof(T).GetMethod(name).MakeGenericMethod(new[] { type });
        }

    }
}
