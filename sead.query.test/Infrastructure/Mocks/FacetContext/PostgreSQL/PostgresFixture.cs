// using DotNet.Testcontainers.Builders;
// using DotNet.Testcontainers.Containers;
// using DotNet.Testcontainers.Configurations;
// using System.Threading.Tasks;
// using Xunit;
// using System.IO;
// using Npgsql;
// using SeadQueryCore;
// using System;
// using SQT.Scaffolding;

// namespace SQT.Infrastructure;

// #pragma warning disable CS1998

// public class PostgresFixture : IAsyncLifetime
// {
//     private static PostgreSqlTestcontainer _container;
//     private static bool _containerInitialized;
//     private static readonly object _lock = new object();
//     public PostgresFixture()
//     {
//         Options = SettingFactory.DefaultSettings;
//     }

//     public PostgreSqlTestcontainer Container => _container;
//     public ISetting Options { get; private set; }

//     public string ConnectionString => Container.ConnectionString;

//     public async Task InitializeDatabaseAsync(string connectionString, string sqlFilePath)
//     {
//         var sql = await File.ReadAllTextAsync(sqlFilePath);

//         await using var conn = new NpgsqlConnection(connectionString);
//         await conn.OpenAsync();

//         await using var cmd = new NpgsqlCommand(sql, conn);
//         await cmd.ExecuteNonQueryAsync();
//     }

//     public async Task InitializeAsync()
//     {
//         if (_containerInitialized) return;

//         lock (_lock)
//         {
//             if (_containerInitialized) return;

//             int port = int.Parse(Options.Store.Port);
//             var config = new PostgreSqlTestcontainerConfiguration
//             {
//                 Database = Options.Store.Database,
//                 Username = Environment.GetEnvironmentVariable("QueryBuilderSetting__Store__Username"),
//                 Password = Environment.GetEnvironmentVariable("QueryBuilderSetting__Store__Password"),
//                 Port = port
//             };
//             _container = new TestcontainersBuilder<PostgreSqlTestcontainer>()
//                 .WithDatabase(config)
//                 .WithImage("postgres:15-alpine")
//                 .WithCleanUp(true)
//                 .WithExposedPort(port)
//                 // don't bind to a fixed host port — let it pick one
//                 //.WithPortBinding(5432, assignRandomHostPort: true)
//                 //.WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(port))
//                 // .WithBindMount("/var/lib/postgresql/data", "/var/lib/postgresql/data") // Persistent data for speed
//                 .Build();

//             _container.StartAsync().Wait();

//             SetupDatabase().ConfigureAwait(false).GetAwaiter().GetResult();
//             _containerInitialized = true;
//         }


//     }

//     private async Task SetupDatabase()
//     {
//         var schemaFilePath = ScaffoldUtility.GetPostgresDataFolder();

//         foreach (var file in Directory.EnumerateFiles(schemaFilePath, "*.sql", SearchOption.TopDirectoryOnly))
//         {
//             await InitializeDatabaseAsync(Container.ConnectionString, file);
//         }
//     }

//     public Task DisposeAsync() => Task.CompletedTask;

//     // public async Task DisposeAsync()
//     // {
//     //     await Container.StopAsync();
//     // }
// }

// #pragma warning restore CS1998
