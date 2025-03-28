﻿using Microsoft.EntityFrameworkCore;
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

    public static class IEnumerableExtensions
    {
        public static string BuildString<T>(this IEnumerable<T> self, string delim = ",", string apos = "")
        {
            return string.Join(delim, self.Select(x => $"{apos}{x}{apos}"));
        }
    }

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

        public static DbContextOptionsBuilder<FacetContext> GetDbContextOptionBuilder(string hostName, string databaseName)
        {
            var defaultSettings = new Dictionary<string, string>
            {
                { "QueryBuilderSetting:Store:Host",     hostName     },
                { "QueryBuilderSetting:Store:Database", databaseName }
            };
            var connectionString = ConnectionStringFactory.Create(defaultSettings);
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
            using (StreamWriter file = new StreamWriter(filename))
            {
                file.Write(data);
            }
        }

        public static ICollection<Type> GetModelTypes()
        {
            return [
                    typeof(ResultFieldType),
                    typeof(ResultField),
                    typeof(ResultViewType),
                    typeof(FacetType),
                    typeof(FacetGroup),
                    typeof(Table),
                    typeof(FacetClause),
                    typeof(FacetChild),
                    typeof(FacetTable),
                    typeof(TableRelation),
                    typeof(ResultSpecificationField),
                    typeof(ResultSpecification),
                    typeof(ViewState),
                    typeof(Facet)
                ];
        }

        public static IPathFinder DefaultRouteFinder(IRepositoryRegistry registry)
        {
            return new PathFinder(registry.Relations.GetEdges());
        }
        public static IPathFinder DefaultRouteFinder(IFacetContext testContext)
        {
            using (var registry = new RepositoryRegistry(testContext))
            {
                var g = DefaultRouteFinder(registry);
                return g;
            }
        }
    }
}
