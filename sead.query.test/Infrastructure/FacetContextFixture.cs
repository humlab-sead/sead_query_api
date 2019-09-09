using DataAccessPostgreSqlProvider;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SeadQueryCore;
using SeadQueryTest.Infrastructure.Scaffolding;
using System;
using System.Collections.Generic;
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

    public class FacetContextFixtureSeededByFolder : FacetContextFixture
    {
        public FacetContextFixtureSeededByFolder(string folder) : base(folder)
        {
        }

        protected override void Seed()
        {
            JsonSerializer serializer = new JsonSerializer {
                // serializer.Converters.Add(new JavaScriptDateTimeConverter());
                NullValueHandling = NullValueHandling.Ignore
            };

            var path = DataFolder;
            var reader = new ScaffoldReader();

            List<FacetGroup> facetGroups = new List<FacetGroup>(reader.Deserialize<FacetGroup>(path));
            FacetContext.AddRange(facetGroups);
            FacetContext.AddRange(reader.Deserialize<FacetType>(path));
            FacetContext.AddRange(reader.Deserialize<FacetClause>(path));
            FacetContext.AddRange(reader.Deserialize<FacetTable>(path));
            FacetContext.AddRange(reader.Deserialize<Facet>(path));
            FacetContext.AddRange(reader.Deserialize<ViewState>(path));
            FacetContext.AddRange(reader.Deserialize<GraphTable>(path));
            FacetContext.AddRange(reader.Deserialize<GraphTableRelation>(path));
            FacetContext.AddRange(reader.Deserialize<ResultViewType>(path));
            FacetContext.AddRange(reader.Deserialize<ResultFieldType>(path));
            FacetContext.AddRange(reader.Deserialize<ResultAggregateField>(path));
            FacetContext.AddRange(reader.Deserialize<ResultField>(path));
            FacetContext.AddRange(reader.Deserialize<ResultAggregate>(path));

            FacetContext.SaveChanges();
 
        }

    }
}
