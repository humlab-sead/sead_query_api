using System;
using System.Data.Common;
using Microsoft.EntityFrameworkCore;

namespace SQT.Infrastructure;

/// <summary>
/// Fixture for creating SQLite database contexts that uses the same backend connection with seeded JSON data.
/// This fixture is used for testing purposes and provides a context with
/// pre-populated data from JSON files.
/// It implements IDisposable to ensure proper cleanup of the database connection.
/// </summary>
public class SqliteFacetContextFixture : IDisposable
{
    private static DbConnection _connection;
    private static DbContextOptions<InMemoryJsonSeededFacetContext> _options;
    private static JsonFacetContextDataFixture _fixture;
    private readonly string _jsonFolder = "Data/FacetDb";
    private bool _created = false;

    public SqliteFacetContextFixture()
    {
        if (_created)
            return;
        _fixture = new JsonFacetContextDataFixture(_jsonFolder);
        var (options, connection) = new Mocks.SqliteConnectionFactory().CreateDbContextOptionsAsync2<InMemoryJsonSeededFacetContext>().GetAwaiter().GetResult();
        _options = (DbContextOptions<InMemoryJsonSeededFacetContext>)options;
        _connection = connection;
        _connection.Open();
        using var seedContext = CreateContext();
        seedContext.Database.EnsureCreated();
        seedContext.SaveChanges();
        _created = true;
    }

    public InMemoryJsonSeededFacetContext CreateContext()
    {
        return new InMemoryJsonSeededFacetContext((_options, _connection, _fixture));
    }

    public void Dispose()
    {
        _connection.Dispose();
    }
}
