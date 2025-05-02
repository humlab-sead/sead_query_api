using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using DotNet.Testcontainers.Configurations;
using System.Threading.Tasks;
using Xunit;
using System.IO;
using Npgsql;
using SeadQueryCore;
using System;
using SQT;
using SQT.Scaffolding;

public class PostgresTestcontainerFixture : IAsyncLifetime
{
    private static PostgreSqlTestcontainer _container;
    private static bool                  _started;
    public PostgresTestcontainerFixture()
    {
        Options = SettingFactory.GetSettings();
    }
    
    public PostgreSqlTestcontainer Container => _container;
    public ISetting Options { get; private set; }
    
    public string ConnectionString => Container.ConnectionString;

    // FIXME: Use a Singleton pattern to avoid multiple initializations
    // FIXME: Use a custom image with the database already set up (option)
    // FIXME: Mount PGDATA to enable reuse (i.e. caching) of database
    // FIXME: Use a random port to avoid conflicts
    public async Task InitializeDatabaseAsync(string connectionString, string sqlFilePath)
    {
        var sql = await File.ReadAllTextAsync(sqlFilePath);

        await using var conn = new NpgsqlConnection(connectionString);
        await conn.OpenAsync();

        await using var cmd = new NpgsqlCommand(sql, conn);
        await cmd.ExecuteNonQueryAsync();
    }

    public async Task InitializeAsync()
    {
        if (!_started)
        {
            var config = new PostgreSqlTestcontainerConfiguration
            {
                Database = Options.Store.Database,
                Username = Environment.GetEnvironmentVariable("QueryBuilderSetting__Store__Username"),
                Password = Environment.GetEnvironmentVariable("QueryBuilderSetting__Store__Password"),
                Port = int.Parse(Options.Store.Port)
            };
            _container = new TestcontainersBuilder<PostgreSqlTestcontainer>()
                .WithDatabase(config)
                .WithImage("postgres:15-alpine")
                .WithCleanUp(true)
                // don't bind to a fixed host port — let it pick one
                //.WithPortBinding(5432, assignRandomHostPort: true)
                .Build();

            await _container.StartAsync();

            await SetupDatabase();

            _started = true;
        }


    }

    private async Task SetupDatabase()
    {
        var schemaFilePath = Path.Combine(ScaffoldUtility.GetDataFolder("PostgreSQL"), "initdb.d");

        foreach (var file in Directory.EnumerateFiles(schemaFilePath, "*.sql", SearchOption.TopDirectoryOnly))
        {
            await InitializeDatabaseAsync(Container.ConnectionString, file);
        }
    }

    public Task DisposeAsync() => Task.CompletedTask;

    // public async Task DisposeAsync()
    // {
    //     await Container.StopAsync();
    // }
}
