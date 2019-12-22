using System;
using System.Collections.Generic;
using System.Text;
using DataAccessPostgreSqlProvider;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Npgsql;
using SeadQueryCore;

namespace SeadQueryTest.Infrastructure.Scaffolding
{
    public static class ScaffoldConnectionUtility
    {

        public static Dictionary<string, string> DefaultSettings()
        {
            var defaultSettings = new Dictionary<string, string>
            {
                { "QueryBuilderSetting:Facet:DirectCountTable",     "tbl_analysis_entities"                     },
                { "QueryBuilderSetting:Facet:DirectCountColumn",    "tbl_analysis_entities.analysis_entity_id"  },
                { "QueryBuilderSetting:Store:Host",                 "seadserv.humlab.umu.se"                    },
                { "QueryBuilderSetting:Store:Port",                 "5432"                                      },
                { "QueryBuilderSetting:Store:Database",             "sead_staging_test"                         },
                { "QueryBuilderSetting:Store:UseRedisCache",        "false"                                     }
            };
            return defaultSettings;
        }
        public static QueryBuilderSetting GetSettings(Dictionary<string, string> memorySettings = null)
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true)
                .AddEnvironmentVariables()
                .AddInMemoryCollection(memorySettings ?? new Dictionary<string, string>())
                .Build()
                .GetSection("QueryBuilderSetting")
                .Get<QueryBuilderSetting>();
            return config;
        }

        public static string GetConnectionString()
        {
            var settings = GetSettings(DefaultSettings()).Store;
            return new NpgsqlConnectionStringBuilder {
                Host = settings.Host,
                Database = settings.Database,
                Username = settings.Username,
                Password = settings.Password,
                Port = Convert.ToInt32(settings.Port),
                Pooling = false
            }.ConnectionString;
        }

        public static DbContextOptionsBuilder<FacetContext> GetDbContextOptionBuilder()
        {
            var connectionString = ScaffoldConnectionUtility.GetConnectionString();
            var optionsBuilder = new DbContextOptionsBuilder<FacetContext>();
            optionsBuilder.UseNpgsql(connectionString);
            return optionsBuilder;
        }
    }
}
