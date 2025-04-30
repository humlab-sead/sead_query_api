using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using DotNet.Testcontainers.Configurations;
using System.Threading.Tasks;
using Xunit;
using System.IO;
using Npgsql;
using SQT.Infrastructure;

public class PostgresTestcontainerFixture : IAsyncLifetime
{
    public PostgreSqlTestcontainer Container { get; private set; }
    public string ConnectionString => Container.ConnectionString;

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
        var config = new PostgreSqlTestcontainerConfiguration
        {
            Database = "sead_staging",
            Username = "postgres",
            Password = "password",
        };

        Container = new TestcontainersBuilder<PostgreSqlTestcontainer>()
            .WithDatabase(config)
            .WithImage("postgres:15-alpine")
            .WithCleanUp(true)
            .Build();

        await Container.StartAsync();

        var schemaFilePath = Path.Combine(ScaffoldUtility.GetRootFolder(), "Infrastructure/Data/PostgreSQL/initdb.d");

        foreach (var file in Directory.EnumerateFiles(schemaFilePath, "*.sql", SearchOption.TopDirectoryOnly))
        {
            await InitializeDatabaseAsync(Container.ConnectionString, file);
        }
    }

    public async Task DisposeAsync()
    {
        await Container.StopAsync();
    }
    
}

