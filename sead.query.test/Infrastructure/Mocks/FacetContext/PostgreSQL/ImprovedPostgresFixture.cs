using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using DotNet.Testcontainers.Configurations;
using System.Threading.Tasks;
using Xunit;
using System.IO;
using Npgsql;
using SeadQueryCore;
using System;
using SQT.Scaffolding;

namespace SQT.Infrastructure;

public class ImprovedPostgresFixture : IAsyncLifetime
{
    private static readonly Lazy<PostgreSqlTestcontainer> _lazyContainer = new Lazy<PostgreSqlTestcontainer>(CreateContainer);
    private static PostgreSqlTestcontainer Container => _lazyContainer.Value;
    public ISetting Options { get; private set; }

    public string ConnectionString => Container.ConnectionString;

    public ImprovedPostgresFixture()
    {
        Options = SettingFactory.DefaultSettings;
    }

    private static PostgreSqlTestcontainer CreateContainer()
    {
        var options = SettingFactory.DefaultSettings;
        int port = int.Parse(options.Store.Port);
        var config = new PostgreSqlTestcontainerConfiguration
        {
            Database = options.Store.Database,
            Username = Environment.GetEnvironmentVariable("QueryBuilderSetting__Store__Username"),
            Password = Environment.GetEnvironmentVariable("QueryBuilderSetting__Store__Password"),
            Port = port
        };

        return new TestcontainersBuilder<PostgreSqlTestcontainer>()
            .WithDatabase(config)
            .WithImage("postgres:15-alpine")
            .WithCleanUp(true)
            .WithExposedPort(port)
            // .WithPortBinding(5432, assignRandomHostPort: true) // Automatic random port
            // .WithBindMount("/var/lib/postgresql/data", "/var/lib/postgresql/data") // Persistent data for speed
            .Build();
    }

    public async Task InitializeAsync()
    {
        if (!Container.State.Equals(TestcontainersState.Running))
        {
            await Container.StartAsync();
            await SetupDatabase();
        }
    }

    private async Task SetupDatabase()
    {
        var schemaFilePath = ScaffoldUtility.GetPostgresDataFolder();

        foreach (var file in Directory.EnumerateFiles(schemaFilePath, "*.sql", SearchOption.TopDirectoryOnly))
        {
            await InitializeDatabaseAsync(ConnectionString, file);
        }
    }

    private async Task InitializeDatabaseAsync(string connectionString, string sqlFilePath)
    {
        var sql = await File.ReadAllTextAsync(sqlFilePath);

        await using var conn = new NpgsqlConnection(connectionString);
        await conn.OpenAsync();

        // Use a single command for better performance
        await using var cmd = new NpgsqlCommand(sql, conn);
        await cmd.ExecuteNonQueryAsync();
    }

    public async Task DisposeAsync()
    {
        if (Container != null && Container.State == TestcontainersState.Running)
        {
            await Container.StopAsync();
        }
    }
}
