using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using DotNet.Testcontainers.Configurations;
using System.Threading.Tasks;
using Xunit;
using System.IO;
using Npgsql;
using SQT.Scaffolding;
using System;
using System.Linq;

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

            var settingFactory = new SettingFactory();
            var options = settingFactory.GetSettings();
            // int port = int.Parse(options.Store.Port);
            var runId = Guid.NewGuid().ToString("N").Substring(0, 8);
            var config = new PostgreSqlTestcontainerConfiguration
            {
                Database = options.Store.Database,
                Username = Environment.GetEnvironmentVariable("QueryBuilderSetting__Store__Username"),
                Password = Environment.GetEnvironmentVariable("QueryBuilderSetting__Store__Password"),
                Port = 5432
            };
            _container = new TestcontainersBuilder<PostgreSqlTestcontainer>()
                .WithDatabase(config)
                .WithImage("postgis/postgis:16-3.5-alpine")
                .WithName($"sead-query-test-postgres-{runId}")
                .WithCleanUp(true)
                // .WithExposedPort(port)
                .WithPortBinding(5432, assignRandomHostPort: true) // Random Port
                // .WithBindMount("/var/lib/postgresql/data", "/var/lib/postgresql/data") // Optional: Persistent data
                // .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(port))
                .Build();

            _container.StartAsync().Wait();
            Environment.SetEnvironmentVariable("QueryBuilderSetting__Store__Port", _container.GetMappedPublicPort(5432).ToString());
            SettingFactory.DefaultSettings.Store.Port =_container.GetMappedPublicPort(5432).ToString();

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
                for r in (
                    select tablename, schemaname
                    from pg_tables
                    where schemaname = 'facet'
                       or (schemaname = 'public' and tablename like 'tbl_%')
                ) loop
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
            await ExecuteSqlFileAsync(Container.ConnectionString, file);
        }

        // 2) bulk-load public/*.csv
        var csvDir = Path.Combine(schemaFilePath, "csv");
        if (Directory.Exists(csvDir))
        {
            await using var conn = new NpgsqlConnection(Container.ConnectionString);
            await conn.OpenAsync();

            await using var transaction = await conn.BeginTransactionAsync(); // Start transaction

            // Set all constraints deferrable within the transaction
            await using (var cmd = new NpgsqlCommand("set constraints all deferred;", conn, transaction))
            {
                await cmd.ExecuteNonQueryAsync();
            }

            foreach (var csvFile in Directory.EnumerateFiles(csvDir, "*.csv"))
            {
                var tableName = Path.GetFileNameWithoutExtension(csvFile);
                // Note: COPY ... FROM STDIN is a server command; use BeginTextImport
                // FIXME: read header line from CSV file and use it to specify columns in COPY statement
                

                var headerLine = File.ReadLines(csvFile).FirstOrDefault();
                var columns = headerLine.Split(',').Select(c => c.Trim()).ToArray();

                var copySql = $"COPY public.\"{tableName}\" ({string.Join(", ", columns)}) FROM STDIN (FORMAT csv, HEADER true)";
                await using var importer = conn.BeginTextImport(copySql);

                // stream file lines into the import
                using var reader = File.OpenText(csvFile);
                while (!reader.EndOfStream)
                {
                    var line = await reader.ReadLineAsync();
                    // Npgsql importer wants exactly the line breaks
                    await importer.WriteAsync(line + "\n");
                }

                await importer.FlushAsync();  // flush the last batch
            }
            await transaction.CommitAsync(); // Commit transaction, constraints are now checked
        }
    }

    private async Task ExecuteSqlFileAsync(string connectionString, string sqlFilePath)
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
