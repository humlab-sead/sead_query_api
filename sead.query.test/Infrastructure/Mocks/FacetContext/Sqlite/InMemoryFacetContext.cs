using Microsoft.EntityFrameworkCore;
using SeadQueryInfra;
using SQT.Infrastructure;
using SQT.Scaffolding;
using System.Data.Common;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;

namespace SQT.Mocks
{
    public class JsonSeededFacetContext : FacetContext
    {

        public JsonFacetContextDataFixture Fixture { get; }
        private DbConnection Connection { get; }

        public JsonSeededFacetContext(DbContextOptions options, JsonFacetContextDataFixture fixture, DbConnection connection) : base(options)
        {
            Fixture = fixture;
            Connection = connection;
        }

        public JsonSeededFacetContext((DbContextOptions options, JsonFacetContextDataFixture fixture, DbConnection connection) args) : this(args.options, args.fixture, args.connection)
        {
        }

        public static async Task<JsonSeededFacetContext> Create(string jsonFolder)
        {
            var fixture = new JsonFacetContextDataFixture(ScaffoldUtility.GetDataFolder(jsonFolder));
            var (options, connection) = await new SqliteConnectionFactory().CreateDbContextOptionsAsync2();
            return new JsonSeededFacetContext(options, fixture, connection);
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
        public JsonSeededFacetContext ExecuteRawSqlFile(string filename)
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
}
