using System;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using SeadQueryCore;

namespace SeadQueryInfra
{
    public class FacetContextFactory(StoreSetting settings) : IFacetContextFactory
    {
        public StoreSetting Settings { get; } = settings;

        public FacetContext GetInstance()
        {
            return new FacetContext(CreateOptionsBuilder(Settings).Options);
        }

        private DbContextOptionsBuilder CreateOptionsBuilder(StoreSetting settings)
        {
            var optionsBuilder = new DbContextOptionsBuilder();

            var connectionString = new NpgsqlConnectionStringBuilder
            {
                Host = settings.Host,
                Database = settings.Database,
                Username = settings.Username,
                Password = settings.Password,
                Port = Convert.ToInt32(settings.Port),
                Pooling = false
            }.ConnectionString;

            optionsBuilder.UseNpgsql(connectionString);

            return optionsBuilder;
        }
    }
}
