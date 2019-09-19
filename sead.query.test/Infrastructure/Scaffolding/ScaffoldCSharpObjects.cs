using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Autofac;
using Autofac.Features.Indexed;
using Moq;
using SeadQueryCore;
using SeadQueryCore.QueryBuilder;
using SeadQueryCore.Services.Result;
using SeadQueryInfra;
using SeadQueryTest.fixtures;
using SeadQueryTest.Infrastructure;
using Xunit;

namespace SeadQueryTest.Infrastructure.Scaffolding
{
    public class ScaffoldCSharpObjects
    {
        private QueryBuilderSetting mockQueryBuilderSetting;
        private RepositoryRegistry mockRegistry;
        private FacetConfigGenerator facetConfigFixture;
        private ResultConfigGenerator resultConfigFixture;

        public ScaffoldCSharpObjects()
        {
            mockQueryBuilderSetting = new MockOptionBuilder().Build().Value;
            mockRegistry = new RepositoryRegistry(ScaffoldUtility.DefaultFacetContext());
            facetConfigFixture = new fixtures.FacetConfigGenerator(mockRegistry);
            resultConfigFixture = new fixtures.ResultConfigGenerator();
        }

        private string GetTargetFolder()
        {
            string path = AppDomain.CurrentDomain.BaseDirectory;
            var parts = new List<string>(path.Split(Path.DirectorySeparatorChar));
            var pos = parts.FindLastIndex(x => string.Equals("bin", x));
            string root = String.Join(Path.DirectorySeparatorChar.ToString(), parts.GetRange(0, pos));
            return Path.Combine(root, "Infrastructure", "Fixtures", "CSharp");
        }

        private void DumpToFile(object instance,  string filename, DumpOptions options=null)
        {
            options = options ?? new DumpOptions() {
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
            using (var context = ScaffoldUtility.DefaultFacetContext()) {

                var repository = new FacetRepository(context);
                var facets = repository.GetAll();
                var path = Path.Join(GetTargetFolder(), "facets.cs.txt");

                DumpToFile(facets, path);
            }

        }

        [Fact]
        public void GenerateFacetsConfigs()
        {
            // Uri format: "target-facet[@trigger-facet]:(facet-code[@picks])(/facet-code[@picks])*
            var uris = new List<string>() {
                "sites@sites:sites:",
                "sites@sites:sites@1",
                "sites@sites:country@73/sites:",
            };


            // using (var context = ScaffoldUtility.DefaultFacetContext())
            using (var container = new TestDependencyService().Register())
            using (var scope = container.BeginLifetimeScope()) {

                foreach (var uri in uris) {
                    var facetsConfig = facetConfigFixture.GenerateByUri(uri);
                    var uriName = UriName(uri);
                    var path = Path.Join(GetTargetFolder(), $"FacetsConfig_{uriName}.cs.txt");

                    DumpToFile(facetsConfig, path);
                }

            }

        }

        [Theory]
        [InlineData("tabular", "site_level")]
        [InlineData("tabular", "aggregate_all")]
        [InlineData("tabular", "sample_group_level")]
        [InlineData("map", "map_result")]
        public void GenerateResultsConfigs(string viewTypeId, string resultKey)
        {
            // using (var context = ScaffoldUtility.DefaultFacetContext())
            using (var container = new TestDependencyService().Register())
            using (var scope = container.BeginLifetimeScope()) {

                var resultConfig = resultConfigFixture.GenerateConfig(viewTypeId, resultKey);

                var path = Path.Join(GetTargetFolder(), $"ResultConfig_{viewTypeId}_{resultKey}.cs.txt");

                DumpToFile(resultConfig, path);

            }

        }
    }
}

