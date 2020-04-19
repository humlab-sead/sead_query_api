using Microsoft.EntityFrameworkCore;
using Microsoft.Data.Sqlite;
using System.Data.Common;
using SeadQueryInfra;

namespace SQT.Mocks
{

    internal static class SqliteContextOptionsFactory
    {
        public static DbContextOptions Create(DbConnection connection)
        {
            var builder = new DbContextOptionsBuilder<FacetContext>()
                .UseSqlite(connection)
                .EnableSensitiveDataLogging(true);

            return builder.Options;
        }
    }

}
