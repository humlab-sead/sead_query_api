using System.Threading.Tasks;
using Xunit;
using System.IO;
using Npgsql;
using SQT.Scaffolding;
using System;
using Testcontainers.PostgreSql;
using System.Linq;

namespace SQT.Infrastructure;

#pragma warning disable CS1998


public class SmartPostgresFixture : IAsyncLifetime
{
    private static PostgreSqlContainer _container;
    private static bool _containerInitialized = false;
    private static readonly object _lock = new object();
    private static readonly string CachedDataFolder = Path.Combine(ScaffoldUtility.GetProjectRoot(), "tmp", "sead-query-pgdata-cache");


    public string ConnectionString => Container.GetConnectionString();
    public PostgreSqlContainer Container => _container;

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

            var options = new SettingFactory().GetSettings();
            var runId = Guid.NewGuid().ToString("N").Substring(0, 8);

            Directory.CreateDirectory(CachedDataFolder);

            bool useExistingDatabaseFolder = false; // Directory.Exists(Path.Combine(CachedDataFolder, "global"));
            var uid = ScaffoldUtility.GetHostUserId();
            var gid = ScaffoldUtility.GetHostGroupId();

            _container = new PostgreSqlBuilder()
                .WithImage("postgis/postgis:16-3.5-alpine")
                .WithName($"sead-query-test-postgres-{runId}")
                .WithUsername(Environment.GetEnvironmentVariable("QueryBuilderSetting__Store__Username"))
                .WithPassword(Environment.GetEnvironmentVariable("QueryBuilderSetting__Store__Password"))
                .WithDatabase(options.Store.Database)
                // .WithHostname("testcontainer_postgres")
                .WithCleanUp(true)
                .WithPortBinding(5432, assignRandomHostPort: true)
                // .WithBindMount(CachedDataFolder, "/var/lib/postgresql/data", AccessMode.ReadWrite)
                .Build();

            _container.StartAsync().Wait();
            Environment.SetEnvironmentVariable("QueryBuilderSetting__Store__Port", _container.GetMappedPublicPort(5432).ToString());
            SettingFactory.DefaultSettings.Store.Port = _container.GetMappedPublicPort(5432).ToString();

            _containerInitialized = true;

            // Only setup database if cache is missing
            if (!useExistingDatabaseFolder)
            {
                Console.WriteLine("Initializing database for the first time (no cache).");
                SetupDatabase().GetAwaiter().GetResult();
            }
            else
            {
                Console.WriteLine("Using cached database files.");
            }
        }
    }

    public async Task InitializeAsync()
    {
        //await ResetDatabase();
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

        await SetupDatabase();
    }

    private async Task SetupDatabase()
    {
        var schemaFilePath = ScaffoldUtility.GetPostgresDataFolder();

        var files = Directory
            .EnumerateFiles(schemaFilePath, "*.sql", SearchOption.TopDirectoryOnly)
            .OrderBy(Path.GetFileName);

        foreach (var file in files)
        {
            await ExecuteSqlFileAsync(Container.GetConnectionString(), file);
        }

        foreach (var schema in new[] { "public", "facet" })
        {
            await Utility.LoadData(schema, Path.Combine(schemaFilePath, "csv", schema), Container.GetConnectionString());
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
    }
}

#pragma warning restore CS1998
