using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Npgsql;
using SeadQueryCore;
using System;
using System.Collections.Generic;
using Xunit;
using SeadQueryTest.Infrastructure.Scaffolding;
using DataAccessPostgreSqlProvider;
using System.IO;

namespace Scaffolding.Infrastructure
{
    public class ScaffoldJsonObjects
    {
        private string GetTargetFolder()
        {
            string root = ScaffoldUtility.GetRootFolder();
            return Path.Combine(root, "Infrastructure", "Data");
        }

        
        [Fact]
        public void WriteDbEntitiesFromContextToFiles()
        {
            DbContextOptionsBuilder<FacetContext> optionsBuilder = ScaffoldConnectionUtility.GetDbContextOptionBuilder();

            JsonSerializer serializer = CreateSerializer();

            var outputPath = GetTargetFolder();
            using (var context = new FacetContext(optionsBuilder.Options)) {
                new ScaffoldWriter(serializer).SerializeTypesToPath(context, ScaffoldUtility.GetModelTypes(), outputPath);
            }
        }

        private static JsonSerializer CreateSerializer()
        {
            JsonSerializer serializer = new JsonSerializer {
                // serializer.Converters.Add(new JavaScriptDateTimeConverter());
                NullValueHandling = NullValueHandling.Ignore,
                Formatting = Formatting.Indented,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };
            return serializer;
        }

        [Fact]
        public void ReadEntitiesFromFileToContext()
        {
            DbContextOptionsBuilder<FacetContext> optionsBuilder = ScaffoldConnectionUtility.GetDbContextOptionBuilder();

            JsonSerializer serializer = new JsonSerializer {
                // serializer.Converters.Add(new JavaScriptDateTimeConverter());
                NullValueHandling = NullValueHandling.Ignore
            };

            var path = GetTargetFolder();
            var reader = new ScaffoldReader();
            using (var context = new FacetContext(optionsBuilder.Options)) {
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


