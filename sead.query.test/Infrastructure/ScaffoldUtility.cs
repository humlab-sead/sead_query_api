using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Npgsql;
using SeadQueryCore;
using SeadQueryInfra;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SQT.Infrastructure
{
    public static class ScaffoldUtility
    {
        public static DumpOptions GetDefaultDumpOptions() => new DumpOptions()
            {
                DumpStyle = DumpStyle.CSharp,
                IndentSize = 1,
                IndentChar = '\t',
                LineBreakChar = Environment.NewLine,
                SetPropertiesOnly = false,
                MaxLevel = 10, // int.MaxValue,
                ExcludeProperties = new HashSet<string>() { "Facets", "Tables", "Facet", "DomainFacet", "TargetFacet" },
                PropertyOrderBy = null,
                IgnoreDefaultValues = false
            };

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

        public static string JsonDataFolder()
        {
            return Path.Combine(ScaffoldUtility.GetRootFolder(), "Infrastructure", "Data", "Json");
        }

        public static string Dump(object instance, DumpOptions options = null)
        {
            options ??= GetDefaultDumpOptions();
            var data = ObjectDumper.Dump(instance, options);
            return data;
        }

        public static void Dump(object instance, string filename, DumpOptions options = null)
        {
            options ??= GetDefaultDumpOptions();
            var data = ObjectDumper.Dump(instance, options);
            using (StreamWriter file = new StreamWriter(filename)) {
                file.Write(data);
            }
        }

        public static ICollection<Type> GetModelTypes()
        {
            return new List<Type>() {
                    typeof(ResultFieldType),
                    typeof(ResultField),
                    typeof(ResultViewType),
                    typeof(FacetType),
                    typeof(FacetGroup),
                    typeof(Table),
                    typeof(FacetClause),
                    typeof(FacetChild),
                    typeof(FacetTable),
                    typeof(Facet),
                    typeof(TableRelation),
                    typeof(ResultAggregateField),
                    typeof(ResultAggregate),
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
