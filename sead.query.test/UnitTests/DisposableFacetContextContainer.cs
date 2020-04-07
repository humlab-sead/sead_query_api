using Microsoft.EntityFrameworkCore;
using SeadQueryInfra;
using SeadQueryTest.Infrastructure;
using SeadQueryTest.Mocks;
using System;
using System.Data.Common;
using Xunit;

namespace SeadQueryTest
{

    [Collection("Json seeded context")]
    public class DisposableFacetContextContainer : IDisposable
    {
        public readonly JsonSeededFacetContextFixture Fixture;
        public readonly DbConnection Connection;
        public readonly DbContextOptions Options;
        public readonly FacetContext Context;
        public readonly RepositoryRegistry Registry;

        public DisposableFacetContextContainer(JsonSeededFacetContextFixture fixture)
        {
            Fixture = fixture;
            Connection = SqliteConnectionFactory.CreateAndOpen();
            Options = SqliteContextOptionsFactory.Create(Connection);
            Context = JsonSeededFacetContextFactory.Create(Options, fixture);
            Registry = new RepositoryRegistry(Context);
        }

        public void Dispose()
        {
            Connection.Close();
            Context.Dispose();
            Registry.Dispose();
            Connection.Dispose();
        }
    }
}
