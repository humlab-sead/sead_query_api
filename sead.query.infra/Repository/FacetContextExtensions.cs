using Microsoft.EntityFrameworkCore;
using System;

namespace SeadQueryInfra
{
    public static class FacetContextExtensions
    {
        public static object SetEx(this DbContext context, Type type)
        {
            return typeof(DbContext).GetMethod("Set").MakeGenericMethod(new[] { type }).Invoke(context, Array.Empty<object>());;
        }

    }
}
