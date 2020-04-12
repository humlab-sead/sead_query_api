using Microsoft.EntityFrameworkCore;
using SeadQueryInfra;
using SeadQueryTest.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SeadQueryTest.Mocks
{
    public class JsonSeededFacetContext : FacetContext
    {
        public JsonSeededFacetContextFixture Fixture { get; }

        public JsonSeededFacetContext(DbContextOptions options, JsonSeededFacetContextFixture fixture) : this(options)
        {
            Fixture = fixture;
        }

        public JsonSeededFacetContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            foreach (var type in ScaffoldUtility.GetModelTypes()) {
                builder.Entity(type).HasData(Fixture.Items[type]);
            }
        }
    }
}
