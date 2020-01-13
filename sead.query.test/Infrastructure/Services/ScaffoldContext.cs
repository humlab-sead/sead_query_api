using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using SeadQueryCore;

namespace SeadQueryTest.Infrastructure.Scaffolding
{

    public class ScaffoldContext : DataAccessPostgreSqlProvider.FacetContext
    {
        public ScaffoldContext(DbContextOptions<DataAccessPostgreSqlProvider.FacetContext> options) : base(options) {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                throw new NotSupportedException("You must supply an already configured DbContextOptionsBuilder!");
            }
        }

    }
}
