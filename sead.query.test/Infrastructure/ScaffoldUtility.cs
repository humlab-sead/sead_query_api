using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Npgsql;
using SeadQueryCore;
using SeadQueryInfra;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SeadQueryTest.Infrastructure
{
    public static class ScaffoldUtility
    {

        public static DbContextOptionsBuilder<FacetContext> GetDbContextOptionBuilder()
        {
            var connectionString = ConnectionStringFactory.Create();
            var optionsBuilder = new DbContextOptionsBuilder<FacetContext>();
            optionsBuilder.UseNpgsql(connectionString);
            return optionsBuilder;
        }

        public static string GetRootFolder()
        {
            string path = AppDomain.CurrentDomain.BaseDirectory;
            var parts = new List<string>(path.Split(Path.DirectorySeparatorChar));
            var pos = parts.FindLastIndex(x => string.Equals("bin", x));
            string root = String.Join(Path.DirectorySeparatorChar.ToString(), parts.GetRange(0, pos));
            return root;
        }

        public static void Dump(object instance, string filename, DumpOptions options = null)
        {
            options ??= new DumpOptions() {
                DumpStyle = DumpStyle.CSharp,
                IndentSize = 1,
                IndentChar = '\t',
                LineBreakChar = Environment.NewLine,
                SetPropertiesOnly = false,
                MaxLevel = 10, // int.MaxValue,
                ExcludeProperties = new HashSet<string>() { "Facets", "Tables", "Facet", "TargetFacet", "TriggerFacet" },
                PropertyOrderBy = null,
                IgnoreDefaultValues = false
            };

            var data = ObjectDumper.Dump(instance, options);
            using (StreamWriter file = new StreamWriter(filename)) {
                file.Write(data);
            }
        }

        public static ICollection<Type> GetModelTypes()
        {
            return new List<Type>() {
                    typeof(Facet),
                    typeof(FacetClause),
                    typeof(FacetGroup),
                    typeof(FacetTable),
                    typeof(FacetType),
                    typeof(Table),
                    typeof(TableRelation),
                    typeof(ResultAggregate),
                    typeof(ResultAggregateField),
                    typeof(ResultField),
                    typeof(ResultFieldType),
                    typeof(ResultViewType),
                    typeof(ViewState)
                };
        }

        public static IFacetsGraph DefaultFacetsGraph(IRepositoryRegistry registry)
        {
            var factory = new FacetGraphFactory(registry);
            var g = factory.Build();
            return g;
        }
        public static IFacetsGraph DefaultFacetsGraph(IFacetContext testContext)
        {
            using (var registry = new RepositoryRegistry(testContext)) {
                var g = DefaultFacetsGraph(registry);
                return g;
            }
        }

    }
}
