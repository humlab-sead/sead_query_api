using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using SeadQueryInfra;
using SQT.Infrastructure;
using SQT.Mocks;

namespace SQT.Scaffolding;

/// <summary>
/// Options for configuring an in-memory FacetContext.
/// </summary>
public class InMemoryFacetContextOptions
{
    /// <summary>
    /// Base folder where CSV files are located.
    /// </summary>
    public string Folder { get; set; }

    /// <summary>
    /// Database schema to seed (e.g. "public").
    /// </summary>
    public string Schema { get; set; }
}

/// <summary>
/// In-memory EF Core context using SQLite and CSV-seeded data fixtures.
/// </summary>
public class InMemoryFacetContext : FacetContext, IDisposable
{
    private readonly IFacetContextDataFixture _fixture;
    private readonly InMemoryFacetContextOptions _options;
    private readonly DbConnection _connection;
    private readonly IReadOnlyDictionary<Type, string> _tableMap;

    public InMemoryFacetContext(
        DbContextOptions<FacetContext> options,
        IFacetContextDataFixture fixture,
        InMemoryFacetContextOptions settings,
        DbConnection connection,
        IReadOnlyDictionary<Type, string> tableMap
    )
        : base(options)
    {
        _fixture = fixture ?? throw new ArgumentNullException(nameof(fixture));
        _options = settings ?? throw new ArgumentNullException(nameof(settings));
        _connection = connection ?? throw new ArgumentNullException(nameof(connection));
        _tableMap = tableMap ?? throw new ArgumentNullException(nameof(tableMap));
    }

    public static async Task<InMemoryFacetContext> CreateAsync(
        InMemoryFacetContextOptions settings,
        IFacetContextDataFixture fixture = null
    )
    {
        ArgumentNullException.ThrowIfNull(settings);

        // Open SQLite in-memory
        var sqlite = new SqliteConnection("DataSource=:memory:;Foreign Keys=True");
        await sqlite.OpenAsync();

        var options = new DbContextOptionsBuilder<FacetContext>().UseSqlite(sqlite).Options;

        fixture ??= new CsvFacetContextDataFixture();

        // Build table map once
        IReadOnlyDictionary<Type, string> tableMap;
        await using (var tempCtx = new FacetContext(options))
        {
            tableMap = tempCtx
                .Model.GetEntityTypes()
                .Where(et => et.GetSchema() == settings.Schema)
                .ToDictionary(
                    et => et.ClrType,
                    et =>
                        et.GetTableName()
                        ?? throw new InvalidOperationException(
                            $"Entity {et.ClrType.Name} has no table mapping."
                        )
                );
        }

        InMemoryFacetContext context = null;
        try
        {
            context = new InMemoryFacetContext(options, fixture, settings, sqlite, tableMap);
            await context.Database.EnsureCreatedAsync();
            return context;
        }
        catch
        {
            context?.Dispose();
            throw;
        }
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        var items = _fixture.Load(_options.Folder, _options.Schema, _tableMap);
        foreach (var kvp in items)
            builder.Entity(kvp.Key).HasData(kvp.Value);
    }

    // ... remaining methods unchanged ...
}

// Registration in DI (e.g. Startup.cs / Program.cs):
// services.Configure<InMemoryFacetContextOptions>(Configuration.GetSection("InMemoryFacet"));
// services.AddSingleton<IFacetContextDataFixture, CsvFacetContextDataFixture>();
// services.AddSingleton<DbConnection>(sp => {
//     var sqlite = new SqliteConnection("DataSource=:memory:;Foreign Keys=True");
//     sqlite.Open();
//     return sqlite;
// });
// services.AddDbContext<FacetContext, InMemoryFacetContext>(
//     (sp, opts) => {
//         var settings = sp.GetRequiredService<IOptions<InMemoryFacetContextOptions>>().Value;
//         var fixture  = sp.GetRequiredService<IFacetContextDataFixture>();
//         var conn     = sp.GetRequiredService<DbConnection>();
//         // Build tableMap once and cache
//         var tableMap = opts.Model.GetEntityTypes()\
//             .Where(et => et.GetSchema() == settings.Schema)
//             .ToDictionary(et => et.ClrType, et => et.GetTableName());
//         return InMemoryFacetContext.CreateAsync(settings, fixture).GetAwaiter().GetResult();
//     });

// public class InMemoryJsonSeededFacetContext : InMemoryFacetContext
// {
//     public InMemoryJsonSeededFacetContext(string folder)
//         : this(
//             new SqliteConnectionFactory()
//                 .CreateDbContextOptionsAsync2<InMemoryJsonSeededFacetContext>()
//                 .GetAwaiter()
//                 .GetResult(),
//             folder
//         ) { }

//     public InMemoryJsonSeededFacetContext(
//         (DbContextOptions options, DbConnection connection) args,
//         string folder
//     )
//         : base(args.options, new JsonFacetContextDataFixture(folder), args.connection) { }

//     public InMemoryJsonSeededFacetContext(
//         (
//             DbContextOptions options,
//             DbConnection connection,
//             JsonFacetContextDataFixture fixture
//         ) args
//     )
//         : base(args.options, args.fixture, args.connection) { }
// }

// public class SqliteFacetContext : InMemoryJsonSeededFacetContext
// {
//     private static bool _created;

//     public SqliteFacetContext()
//         : base(ScaffoldUtility.GetInMemoryDataFolder("Data/FacetDb"))
//     {
//         if (!_created)
//         {
//             Database.EnsureCreated();
//             _created = true;
//         }
//     }
// }
