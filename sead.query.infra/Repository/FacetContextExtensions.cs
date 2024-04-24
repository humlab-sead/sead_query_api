using System;
using Microsoft.EntityFrameworkCore;

namespace SeadQueryInfra
{
    public static class FacetContextExtensions
    {
        public static object SetEx(this DbContext context, Type type)
        {
            return typeof(DbContext).GetMethod("Set", Type.EmptyTypes).MakeGenericMethod(new[] { type }).Invoke(context, []);
        }
    }
}
