using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using DotNet.Testcontainers.Configurations;
using System.Threading.Tasks;
using Xunit;
using System.IO;
using Npgsql;
using SQT.Scaffolding;

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

            _container = new TestcontainersBuilder<PostgreSqlTestcontainer>()
                .WithDatabase(new PostgreSqlTestcontainerConfiguration
                {
                    Database = "testdb",
                    Username = "testuser",
                    Password = "testpassword"
                })
                .WithImage("postgres:15-alpine")
                .WithCleanUp(true)
                .WithExposedPort(5432)
                .WithPortBinding(5432, assignRandomHostPort: true) // Random Port
                .WithBindMount("/var/lib/postgresql/data", "/var/lib/postgresql/data") // Optional: Persistent data
                .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(5432))
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
            DO $$
            DECLARE
                r RECORD;
            BEGIN
                FOR r IN (SELECT tablename FROM pg_tables WHERE schemaname = 'public') LOOP
                    EXECUTE 'DROP TABLE IF EXISTS ' || quote_ident(r.tablename) || ' CASCADE';
                END LOOP;
            END $$;
        ";
        await using var cmd = new NpgsqlCommand(resetSql, conn);
        await cmd.ExecuteNonQueryAsync();

        // Re-seed database (load schema)
        await SetupDatabase();
    }

    private async Task SetupDatabase()
    {
        var schemaFilePath = Path.Combine(ScaffoldUtility.GetDataFolder("PostgreSQL"), "initdb.d");
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
