using Microsoft.EntityFrameworkCore;
using SeadQueryInfra;
using SQT.Infrastructure;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;

namespace SQT.Mocks
{
    public class JsonSeededFacetContext : FacetContext
    {
        public JsonFacetContextFixture Fixture { get; }

        public JsonSeededFacetContext(DbContextOptions options, JsonFacetContextFixture fixture) : base(options)
        {
            Fixture = fixture;
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
    }
}
