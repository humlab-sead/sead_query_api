using System;
using System.Collections.Generic;
using System.IO;
using SeadQueryCore;
using SeadQueryInfra;
using SeadQueryTest.Fixtures;
using SeadQueryTest.Infrastructure.Scaffolding;
using Xunit;

namespace Scaffolding.Infrastructure
{
    public class ScaffoldCSharpObjects
    {
        private QueryBuilderSetting mockQueryBuilderSetting;
        private RepositoryRegistry mockRegistry;
        // private ScaffoldResultConfig resultConfigFixture;

        public ScaffoldCSharpObjects()
        {
            mockQueryBuilderSetting = new SeadQueryTest.MockOptionBuilder().Build().Value;
            mockRegistry = new RepositoryRegistry(ScaffoldUtility.DefaultFacetContext());
        }

        private string GetTargetFolder()
        {
            string root = ScaffoldUtility.GetRootFolder();
            return Path.Combine(root, "Infrastructure", "Data", "CSharp");
        }

        private string UriName(string uri)
        {
            string invalidChars = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars());
            var uriName = uri.Replace(":", "=").Replace("/", "+");
            foreach (var c in invalidChars) {
                uriName = uriName.Replace(c.ToString(), "");
            }
            return uriName;
        }

        [Fact]
        public void GenerateFacets()
        {
            var options = new DumpOptions() {
                DumpStyle = DumpStyle.CSharp,
                IndentSize = 1,
                IndentChar = '\t',
                LineBreakChar = Environment.NewLine,
                SetPropertiesOnly = false,
                MaxLevel = 10, // int.MaxValue,
                ExcludeProperties = new HashSet<string>() {
                    "Facets",
                    "Facet",
                    "TargetTable",
                    "QueryCriteria",
                    "FacetGroupKey",
                    "FacetTypeKey",
                    "FacetTypeKey",
                },
                PropertyOrderBy = null,
                IgnoreDefaultValues = false
            };

            using (var context = ScaffoldUtility.DefaultFacetContext()) {

                var repository = new FacetRepository(context);
                var facets = repository.GetAll();
                var path = Path.Join(GetTargetFolder(), "Facet.cs.txt");

                ScaffoldUtility.Dump(facets, path, options);
            }

        }

        [Fact]
        public void GenerateFacetsConfigs()
        {
            var scaffolder = new SeadQueryTest.Fixtures.ScaffoldFacetsConfig(mockRegistry);

            // Uri format: "target-facet[@trigger-facet]:(facet-code[@picks])(/facet-code[@picks])*
            var uris = new List<string>() {
                // "sites@sites:sites:",
                // "sites@sites:sites@1",
                // "sites@sites:country@73/sites:",
                "tbl_denormalized_measured_values_33_0:tbl_denormalized_measured_values_33_0@(3,52)"
            };


            // using (var context = ScaffoldUtility.DefaultFacetContext())

            foreach (var uri in uris) {
                var facetsConfig = scaffolder.Create(uri);
                var uriName = UriName(uri);
                var path = Path.Join(GetTargetFolder(), $"FacetsConfig_{uriName}.cs.txt");

                ScaffoldUtility.Dump(facetsConfig, path);
            }
        }

        [Fact]
        public void GenerateQuerySetups()
        {
            var scaffolder = new SeadQueryTest.Fixtures.ScaffoldQuerySetup(mockRegistry);

            // Uri format: "target-facet[@trigger-facet]:(facet-code[@picks])(/facet-code[@picks])*
            var uris = new List<string>() {
                "sites@sites:sites:",
                "sites@sites:sites@1",
                "sites@sites:country@73/sites:",
                "tbl_denormalized_measured_values_33_0:tbl_denormalized_measured_values_33_0@(110,2904)"
            };
            var options = new DumpOptions() {
                DumpStyle = DumpStyle.CSharp,
                IndentSize = 1,
                IndentChar = '\t',
                LineBreakChar = Environment.NewLine,
                SetPropertiesOnly = false,
                MaxLevel = 10, // int.MaxValue,
                ExcludeProperties = new HashSet<string>() {
                    "Facet",
                    "SourceName",
                    "TargetName",
                    "Key",
                    "CategoryTextFilter"
                },
                PropertyOrderBy = null,
                IgnoreDefaultValues = false
            };

            foreach (var uri in uris) {

                var querySetup = scaffolder.Scaffold(uri);

                var path = Path.Join(GetTargetFolder(), $"QuerySetup_{UriName(uri)}.cs.txt");
                ScaffoldUtility.Dump(querySetup, path, options);
            }
        }

        [Theory]
        [InlineData("tabular", "site_level")]
        [InlineData("tabular", "aggregate_all")]
        [InlineData("tabular", "sample_group_level")]
        [InlineData("map", "map_result")]
        public void GenerateResultsConfigs(string viewTypeId, string resultKey)
        {
            var scaffolder = new SeadQueryTest.Fixtures.ScaffoldResultConfig();
            var resultConfig = scaffolder.GenerateConfig(viewTypeId, resultKey);

            var path = Path.Join(GetTargetFolder(), $"ResultConfig_{viewTypeId}_{resultKey}.cs.txt");

            ScaffoldUtility.Dump(resultConfig, path);
        }
    }
}

