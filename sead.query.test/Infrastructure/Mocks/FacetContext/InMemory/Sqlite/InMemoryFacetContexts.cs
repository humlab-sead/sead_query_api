using Microsoft.EntityFrameworkCore;
using SeadQueryInfra;
using SQT.Infrastructure;
using SQT.Mocks;
using SQT.Scaffolding;
using System.Data.Common;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;

namespace SQT.Infrastructure
{

    
    public class InMemoryFacetContext : FacetContext
    {

        public JsonFacetContextDataFixture Fixture { get; }
        private DbConnection Connection { get; }

        public InMemoryFacetContext(DbContextOptions options, JsonFacetContextDataFixture fixture, DbConnection connection) : base(options)
        {
            Fixture = fixture;
            Connection = connection;
        }

        public InMemoryFacetContext((DbContextOptions options, JsonFacetContextDataFixture fixture, DbConnection connection) args) : this(args.options, args.fixture, args.connection)
        {
        }

        public static async Task<InMemoryFacetContext> Create(string jsonFolder)
        {
            var fixture = new JsonFacetContextDataFixture(ScaffoldUtility.GetInMemoryDataFolder());
            var (options, connection) = await new SqliteConnectionFactory().CreateDbContextOptionsAsync2<InMemoryFacetContext>();
            return new InMemoryFacetContext(options, fixture, connection);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            foreach (var type in ScaffoldUtility.GetModelTypes())
            {
                builder.Entity(type).HasData(Fixture.Items[type]);
            }
        }

        /// <summary>
        /// Create the target model of the Facet system (i.e. the public schema)
        /// </summary>
        /// <param name="filename"></param>
        public InMemoryFacetContext ExecuteRawSqlFile(string filename)
        {
            if (!File.Exists(filename))
                throw new FileNotFoundException(filename);

            var schema_sql = GetFileContent(filename);

            Database.ExecuteSqlRaw(schema_sql);

            return this;
        }

        private static string GetFileContent(string filename) => filename.EndsWith(".gz") ? ReadGZip(filename) : File.ReadAllText(filename);

        private static string ReadGZip(string filename)
        {
            string schema_ddl;
            using (Stream fileStream = File.OpenRead(filename))
            using (StreamReader reader = new StreamReader(new GZipStream(fileStream, CompressionMode.Decompress)))
                schema_ddl = reader.ReadToEnd();
            return schema_ddl;
        }

        public override void Dispose()
        {
            base.Dispose();
            Connection?.Dispose();
        }
    }


    public class InMemoryJsonSeededFacetContext : InMemoryFacetContext
    {
        public InMemoryJsonSeededFacetContext(string jsonFolder) : this(
            new SqliteConnectionFactory().CreateDbContextOptionsAsync2<InMemoryJsonSeededFacetContext>().GetAwaiter().GetResult(),
            jsonFolder
            )
        {
        }

        public InMemoryJsonSeededFacetContext((DbContextOptions options, DbConnection connection) args, string jsonFolder) : base(
            args.options,
            new JsonFacetContextDataFixture(ScaffoldUtility.GetInMemoryDataFolder(jsonFolder)),
            args.connection
        )
        {
        }

        public InMemoryJsonSeededFacetContext((DbContextOptions options, DbConnection connection, JsonFacetContextDataFixture fixture) args) : base(
            args.options,
            args.fixture,
            args.connection
        )
        {
        }
    }

    public class SqliteFacetContext : InMemoryJsonSeededFacetContext
    {
        private static bool _created;
        public SqliteFacetContext() : base("Data/FacetDb")
        {
            if (!_created) {
                Database.EnsureCreated();
                _created = true;
            }
        }
    }

}
