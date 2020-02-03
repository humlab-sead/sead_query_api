using Microsoft.EntityFrameworkCore;
using Microsoft.Data.Sqlite;
using System.Data.Common;
using SeadQueryInfra;

namespace SeadQueryTest.Mocks
{

    internal static class SqliteInMemoryContextOptionsFactory
    {
        public static DbContextOptions Create(DbConnection connection=null)
        {
            if (connection == null)
                connection = FakeConnectionFactory.Create();

            var builder = new DbContextOptionsBuilder<FacetContext>()
                .UseSqlite(connection);

            return builder.Options;
        }
    }

}
