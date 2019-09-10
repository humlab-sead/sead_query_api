using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Npgsql;
using sead.query.test.data.Infrastructure;
using SeadQueryCore;
using System;
using System.Collections.Generic;
using Xunit;

namespace sead.query.test.data
{
    public class ScaffoldData
    {
        private Dictionary<string, string> DefaultSettings()
        {
            var defaultSettings = new Dictionary<string, string>
            {
                { "QueryBuilderSetting:Facet:DirectCountTable",     "tbl_analysis_entities"                     },
                { "QueryBuilderSetting:Facet:DirectCountColumn",    "tbl_analysis_entities.analysis_entity_id"  },
                { "QueryBuilderSetting:Facet:IndirectCountTable",   "tbl_dating_periods"                        },
                { "QueryBuilderSetting:Facet:IndirectCountColumn",  "tbl_dating_periods.dating_period_id"       },
                { "QueryBuilderSetting:Facet:ResultQueryLimit",     "10000"                                     },
                { "QueryBuilderSetting:Facet:CategoryNameFilter",   "true"                                      },
                { "QueryBuilderSetting:Store:Host",                 "seadserv.humlab.umu.se"                                      },
                { "QueryBuilderSetting:Store:Port",                 "5432"                    },
                { "QueryBuilderSetting:Store:Database",             "sead_staging_facet"                        },
                { "QueryBuilderSetting:Store:UseRedisCache",        "false"                                     }
            };
            return defaultSettings;
        }
        public QueryBuilderSetting GetSettings(Dictionary<string, string> memorySettings = null)
        {
            var config = new ConfigurationBuilder()
                .AddInMemoryCollection(memorySettings ?? new Dictionary<string, string>())
                .AddJsonFile("appsettings.json", optional: true)
                .AddEnvironmentVariables()
                .Build()
                .GetSection("QueryBuilderSetting")
                .Get<QueryBuilderSetting>();
            return config;
        }

        private string GetConnectionString()
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

        [Fact]
        public void WriteDbEntitiesFromContextToFiles()
        {
            var connectionString = GetConnectionString();
            var optionsBuilder = new DbContextOptionsBuilder<ScaffoldedFacetContext>();
            optionsBuilder.UseNpgsql(connectionString);

            JsonSerializer serializer = new JsonSerializer {
                // serializer.Converters.Add(new JavaScriptDateTimeConverter());
                NullValueHandling = NullValueHandling.Ignore,
                Formatting = Formatting.Indented
            };

            var outputPath = ScaffoldUtility.GetTestDataFolder();
            using (var context = new ScaffoldedFacetContext(optionsBuilder.Options)) {
                new ScaffoldWriter(serializer).SerializeTypesToPath(context, ScaffoldUtility.GetModelTypes(), outputPath);
            }
        }

        [Fact]
        public void ReadEntitiesFromFileToContext()
        {
            var connectionString = GetConnectionString();
            var optionsBuilder = new DbContextOptionsBuilder<ScaffoldedFacetContext>();
            optionsBuilder.UseNpgsql(connectionString);

            JsonSerializer serializer = new JsonSerializer {
                // serializer.Converters.Add(new JavaScriptDateTimeConverter());
                NullValueHandling = NullValueHandling.Ignore
            };

            var path = ScaffoldUtility.GetTestDataFolder();
            var reader = new ScaffoldReader();
            using (var context = new ScaffoldedFacetContext(optionsBuilder.Options)) {
                var types = ScaffoldUtility.GetModelTypes();
                var facets = reader.Deserialize<SeadQueryCore.Facet>(path);
            }
        }


    }
}

//AsNoTracking
//ScaffoldUtility.SerializeToFile(serializer, context.Facet, Path.Combine(outputPath, "Facets.json"));
//SerializeToFile(serializer, context.FacetClause, Path.Combine(outputPath, "FacetClause.json"));
//SerializeToFile(serializer, context.FacetGroup, Path.Combine(outputPath, "FacetGroup.json"));
//SerializeToFile(serializer, context.FacetTable, Path.Combine(outputPath, "FacetTable.json"));
//SerializeToFile(serializer, context.FacetType, Path.Combine(outputPath, "FacetType.json"));
//SerializeToFile(serializer, context.GraphNode, Path.Combine(outputPath, "GraphNode.json"));
//SerializeToFile(serializer, context.GraphEdge, Path.Combine(outputPath, "GraphEdge.json"));
//SerializeToFile(serializer, context.ResultAggregate, Path.Combine(outputPath, "ResultAggregate.json"));
//SerializeToFile(serializer, context.ResultAggregateField, Path.Combine(outputPath, "ResultAggregateField.json"));
//SerializeToFile(serializer, context.ResultField, Path.Combine(outputPath, "ResultField.json"));
//SerializeToFile(serializer, context.ResultFieldType, Path.Combine(outputPath, "ResultFieldType.json"));
//SerializeToFile(serializer, context.ResultViewType, Path.Combine(outputPath, "ResultViewType.json"));
//SerializeToFile(serializer, context.ViewState, Path.Combine(outputPath, "ViewState.json"));


