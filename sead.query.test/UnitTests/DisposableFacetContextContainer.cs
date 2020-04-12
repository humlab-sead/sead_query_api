using Microsoft.EntityFrameworkCore;
using SeadQueryCore;
using SeadQueryInfra;
using SeadQueryTest.Infrastructure;
using SeadQueryTest.Mocks;
using System;
using System.Data.Common;
using System.Threading;
using Xunit;

namespace SeadQueryTest
{

    [Collection("JsonSeededFacetContext")]
    public class DisposableFacetContextContainer : IDisposable
    {
        private JsonSeededFacetContextFixture __fixture;

        private Lazy<DbConnection> __DbConnection;
        private Lazy<DbContextOptions> __DbContextOptions;
        private Lazy<FacetContext> __FacetContext;
        private Lazy<RepositoryRegistry> __RepositoryRegistry;
        private Lazy<ISetting> __Settings;

        public virtual JsonSeededFacetContextFixture Fixture => __fixture;
        public virtual ISetting Settings => __Settings.Value;
        public virtual DbConnection DbConnection => __DbConnection.Value;
        public virtual DbContextOptions DbContextOptions => __DbContextOptions.Value;
        public virtual FacetContext FacetContext => __FacetContext.Value;
        public virtual RepositoryRegistry Registry => __RepositoryRegistry.Value;

        protected virtual ISetting CreateSettings()
            => (ISetting)new SettingFactory().Create().Value;

        protected virtual DbConnection CreateDbConnection()
            => SqliteConnectionFactory.CreateAndOpen();

        protected virtual DbContextOptions CreateDbContextOptions()
            => SqliteContextOptionsFactory.Create(DbConnection);

        private FacetContext CreateFacetContext()
            => JsonSeededFacetContextFactory.Create(DbContextOptions, Fixture);

        public virtual RepositoryRegistry CreateRepositoryRegistry()
            => new RepositoryRegistry(FacetContext);

        public DisposableFacetContextContainer(JsonSeededFacetContextFixture fixture)
        {
            __fixture = fixture;
            __DbConnection = new Lazy<DbConnection>(CreateDbConnection);
            __DbContextOptions = new Lazy<DbContextOptions>(CreateDbContextOptions);
            __FacetContext = new Lazy<FacetContext>(CreateFacetContext);
            __RepositoryRegistry = new Lazy<RepositoryRegistry>(CreateRepositoryRegistry);
            __Settings = new Lazy<ISetting>(CreateSettings);
        }

        public void Dispose()
        {
            if (__DbConnection.IsValueCreated)
                DbConnection.Close();

            if (__FacetContext.IsValueCreated)
                FacetContext.Dispose();

            if (__RepositoryRegistry.IsValueCreated)
                Registry.Dispose();

            if (__DbConnection.IsValueCreated)
                DbConnection.Dispose();
        }
    }
}
