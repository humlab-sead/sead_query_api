using Microsoft.EntityFrameworkCore;
using SeadQueryCore;
using SeadQueryTest.Mocks;
using System;
using System.Collections.Generic;
using System.Linq;
using SeadQueryInfra;
using SeadQueryTest.Fixtures;
using System.IO;
using SeadQueryTest.Infrastructure;
using System.Data.Common;

namespace SeadQueryTest.Mocks
{
    internal static class FakeFacetContextFactory
    {
        public static FacetContext Create(DbContextOptions options, IFakeFacetContextSeeder seeder=null)
        {
            if (options == null)
                options = SqliteInMemoryContextOptionsFactory.Create();

            var context = new FacetContext(options);

            if (seeder != null)
                seeder.Seed(context);

            return context;
        }

        public static FacetContext Empty(DbConnection connection)
        {
            var options = SqliteInMemoryContextOptionsFactory.Create(connection);
            var context = new FacetContext(options);
            return context;
        }
    }

    internal static class JsonSeededFacetContextFactory
    {
        public static FacetContext Create(DbConnection connection=null)
        {
            var seeder = new FakeFacetContextJsonSeeder();
            var options = SqliteInMemoryContextOptionsFactory.Create(connection);
            var context = new FacetContext(options);
            seeder.Seed(context);
            return context;
        }
    }
}
