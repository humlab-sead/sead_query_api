using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using SeadQueryInfra;
using System.Data.Common;
using System.Threading.Tasks;

namespace SQT.Mocks
{

    // public class SqliteContextOptionsFactory
    // {
    //     public virtual DbContextOptions Create<T>(DbConnection connection=null) where T : DbContext
    //     {
    //         var builder = new DbContextOptionsBuilder<T>()
    //             .UseSqlite(connection)
    //             .EnableSensitiveDataLogging(true);

    //         return builder.Options;
    //     }
    // }

    internal class SqliteConnectionFactory
    {
        /// <summary>
        /// Asynchronously build DbContextOptions by first creating/opening the connection.
        /// </summary>
        public virtual async Task<DbContextOptions<T>> CreateDbContextOptionsAsync<T>() where T : DbContext
        {
            var (options, _) = await CreateDbContextOptionsAsync2<T>();
            return (DbContextOptions<T>)options;
        }

        public virtual async Task<(DbContextOptions, DbConnection)> CreateDbContextOptionsAsync2<T>() where T : DbContext
        {
            var connection = await CreateAsync().ConfigureAwait(false);
            var builder = new DbContextOptionsBuilder<T>()
                .UseSqlite(connection)
                .EnableSensitiveDataLogging(true);
            return (builder.Options, connection);
        }

        /// <summary>
        /// Synchronous options builder (unchanged).
        /// </summary>
        public virtual DbContextOptions CreateDbContextOptions<T>() where T : DbContext
            => CreateDbContextOptionsAsync<T>().GetAwaiter().GetResult();

        /// <summary>
        /// Asynchronously create and open an in-memory SQLite connection.
        /// </summary>
        public virtual async Task<SqliteConnection> CreateAsync()
        {
            var connection = new SqliteConnection("DataSource=:memory:;Foreign Keys=False");
            await connection.OpenAsync().ConfigureAwait(false);
            return connection;
        }

        /// <summary>
        /// Alias for CreateAsync – returns a generic DbConnection.
        /// </summary>
        public virtual async Task<DbConnection> CreateAndOpenAsync()
            => await CreateAsync();
 
    }
}
