using Microsoft.EntityFrameworkCore;
using Moq;
using SeadQueryInfra;
using SQT.Infrastructure;
using SQT.Mocks;
using System;
using System.Data.Common;

namespace SQT
{
    public class MockerWithJsonFacetContext : MockerWithFacetContext
    {
        private readonly JsonFacetContextDataFixture __fixture;

        private readonly Lazy<DbConnection> __DbConnection;
        private readonly Lazy<DbContextOptions> __DbContextOptions;
        public virtual JsonFacetContextDataFixture Fixture => __fixture;
        public virtual DbConnection DbConnection => __DbConnection.Value;
        public virtual DbContextOptions DbContextOptions => __DbContextOptions.Value;


        public virtual DbConnection CreateDbConnection()
            => SqliteConnectionFactory.CreateAndOpen();

        public virtual DbContextOptions CreateDbContextOptions()
            => SqliteContextOptionsFactory.Create(DbConnection);

        public override FacetContext CreateFacetContext()
            => JsonSeededFacetContextFactory.Create(DbContextOptions, Fixture);

        public MockerWithJsonFacetContext(JsonFacetContextDataFixture fixture) : base()
        {
            __fixture = fixture;
            __DbConnection = new Lazy<DbConnection>(CreateDbConnection);
            __DbContextOptions = new Lazy<DbContextOptions>(CreateDbContextOptions);
        }

        public override void Dispose()
        {
            if (__DbConnection.IsValueCreated)
                DbConnection.Close();

            if (__DbConnection.IsValueCreated)
                DbConnection.Dispose();
            base.Dispose();
        }

    }
}
