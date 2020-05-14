using Microsoft.EntityFrameworkCore;
using SeadQueryInfra;
using SQT.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SQT.Mocks
{
    public class JsonSeededFacetContext : FacetContext
    {
        public JsonSeededFacetContextFixture Fixture { get; }

        public JsonSeededFacetContext(DbContextOptions options, JsonSeededFacetContextFixture fixture) : base(options)
        {
            Fixture = fixture;
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
