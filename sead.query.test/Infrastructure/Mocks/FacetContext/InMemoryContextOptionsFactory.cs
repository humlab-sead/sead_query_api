using Microsoft.EntityFrameworkCore;
using System;

namespace SQT.Mocks
{
    internal static class InMemoryContextOptionsFactory
    {
        public static DbContextOptions Create()
        {
            var builder = new DbContextOptionsBuilder();

            builder.UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .EnableSensitiveDataLogging();

            return builder.Options;
        }
    }

}
