using DataAccessPostgreSqlProvider;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Text;

namespace SeadQueryTest.Infrastructure
{
    public class FacetContextFixture : IDisposable
    {
        public string DataFolder { get; }
        public FacetContext FacetContext { get; private set; }

        public FacetContextFixture(string folder)
        {
            DataFolder = folder;
            var options = new DbContextOptionsBuilder<FacetContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .EnableSensitiveDataLogging()
                .Options;
            FacetContext = new FacetContext(options);
            Seed();
        }

        protected virtual void Seed()
        {
            FacetContext.SaveChanges();
        }

        public void Dispose()
        {
            FacetContext.Dispose();
        }
    }
}
