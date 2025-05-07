using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using DotNet.Testcontainers.Configurations;
using System.Threading.Tasks;
using Xunit;
using System.IO;
using Npgsql;
using SQT.Scaffolding;
using System;

namespace SQT.Infrastructure;

#pragma warning disable CS1998

public class SmartPostgresFixture : IAsyncLifetime
{
    private static PostgreSqlTestcontainer _container;
    private static bool _containerInitialized = false;
    private static readonly object _lock = new object();
    
    public string ConnectionString => Container.ConnectionString;
    public PostgreSqlTestcontainer Container => _container;

    public SmartPostgresFixture()
    {
        EnsureContainerStarted().Wait();
    }

    // Ensures the container is started only once
    
    private async Task EnsureContainerStarted()
    {
        if (_containerInitialized) return;

        lock (_lock)
        {
            if (_containerInitialized) return;

            var options = SettingFactory.GetSettings();
            int port = int.Parse(options.Store.Port);
            var config = new PostgreSqlTestcontainerConfiguration
            {
                Database = options.Store.Database,
                Username = Environment.GetEnvironmentVariable("QueryBuilderSetting__Store__Username"),
                Password = Environment.GetEnvironmentVariable("QueryBuilderSetting__Store__Password"),
                Port = port
            };
            _container = new TestcontainersBuilder<PostgreSqlTestcontainer>()
                .WithDatabase(config)
                .WithImage("postgres:15-alpine")
                .WithName("sead-query-test-postgres")
                .WithCleanUp(true)
                .WithExposedPort(port)
                // .WithPortBinding(5432, assignRandomHostPort: true) // Random Port
                // .WithBindMount("/var/lib/postgresql/data", "/var/lib/postgresql/data") // Optional: Persistent data
                // .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(port))
                .Build();

            _container.StartAsync().Wait();
            _containerInitialized = true;
        }
    }

    public async Task InitializeAsync()
    {
        await ResetDatabase();
    }

    private async Task ResetDatabase()
    {
        await using var conn = new NpgsqlConnection(ConnectionString);
        await conn.OpenAsync();

        // Drop all tables (clean slate)
        var resetSql = @"
            do $$
            declare
                r record;
            begin
                for r in (select tablename from pg_tables where schemaname in ('facet', 'public')) loop
                    execute 'drop table if exists ' || quote_ident(r.schemaname) || '.' || quote_ident(r.tablename) || ' cascade';
                end loop;
            end $$;
        ";
        await using var cmd = new NpgsqlCommand(resetSql, conn);
        await cmd.ExecuteNonQueryAsync();

        // Re-seed database (load schema)
        await SetupDatabase();
    }

    private async Task SetupDatabase()
    {
        var schemaFilePath = ScaffoldUtility.GetPostgresDataFolder();
        foreach (var file in Directory.EnumerateFiles(schemaFilePath, "*.sql", SearchOption.TopDirectoryOnly))
        {
            await InitializeDatabaseAsync(Container.ConnectionString, file);
        }
    }

    private async Task InitializeDatabaseAsync(string connectionString, string sqlFilePath)
    {
        var sql = await File.ReadAllTextAsync(sqlFilePath);

        await using var conn = new NpgsqlConnection(connectionString);
        await conn.OpenAsync();

        await using var cmd = new NpgsqlCommand(sql, conn);
        await cmd.ExecuteNonQueryAsync();
    }

    public async Task DisposeAsync()
    {
        // Optionally keep the container for reuse (fastest)
        // Uncomment to stop and cleanup after all tests
        // if (_container != null && _container.State == TestcontainersStates.Running)
        // {
        //     await _container.StopAsync();
        // }
    }
}

#pragma warning restore CS1998
