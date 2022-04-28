using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.EntityFrameworkCore;
using SeadQueryCore;
using SeadQueryInfra;
using SQT.Mocks;
using Xunit;

namespace SQT.Infrastructure.Scaffolding
{
    [Collection("SeadJsonFacetContextFixture")]
    public class GenerateCSharpFixures : DisposableFacetContextContainer
    {
        private readonly Setting mockQueryBuilderSetting;

        public GenerateCSharpFixures(SeadJsonFacetContextFixture fixture) : base(fixture)
        {
            mockQueryBuilderSetting = new SQT.SettingFactory().Create().Value;
        }

        private string DataFolder()
        {
            string root = ScaffoldUtility.GetRootFolder();
            return Path.Combine(root, "Infrastructure", "Data", "CSharp");
        }

        private string UriName(string uri)
        {
            string invalidChars = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars());
            var uriName = uri.Replace(":", "=").Replace("/", "+");
            foreach (var c in invalidChars)
            {
                uriName = uriName.Replace(c.ToString(), "");
            }
            return uriName;
        }

        //[Fact(Skip = "Not a test. Scaffolds C# facets from database. Stores data in FacetDict.cs.txt")]
        public void ScaffoldCSharpFacetsToFileUsingOnlineDatabase()
        {
            var options = new DumpOptions()
            {
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
                    "TableOrUdfName",
                    "HasAlias",
                    "ResolvedAliasOrTableOrUdfName",
                    "ResolvedTableOrUdfCall",
                    "ResolvedSqlJoinName"
                },
                PropertyOrderBy = null,
                IgnoreDefaultValues = false
            };

            var facets = Registry.Facets.GetAll();

            Dictionary<string, Facet> facetsDict = new Dictionary<string, Facet>();
            foreach (var facet in facets)
            {
                facetsDict.Add(facet.FacetCode, facet);
            }
            var path = Path.Join(DataFolder(), "FacetsDict.cs.txt");

            ScaffoldUtility.Dump(facetsDict, path, options);
        }

        //[Fact(Skip = "Not a test. Scaffolds FacetsConfigs from JSON seeded context")]
        public void ScaffoldCSharpFacetsConfigsToFileUsingJsonSeededFacetContext()
        {
            var scaffolder = new MockFacetsConfigFactory(Registry.Facets);
            // Uri format: "target-facet[@trigger-facet]:(facet-code[@picks])(/facet-code[@picks])*
            var uris = new List<string>() {
                // "sites:sites:",
                // "sites:sites@1",
                // "sites:country@73/sites:",
                "tbl_denormalized_measured_values_33_0:tbl_denormalized_measured_values_33_0@(3,52)"
            };

            foreach (var uri in uris)
            {
                var facetsConfig = scaffolder.Create(uri);
                var uriName = UriName(uri);
                var path = Path.Join(DataFolder(), $"FacetsConfig_{uriName}.cs.txt");

                ScaffoldUtility.Dump(facetsConfig, path);
            }
        }

        //[Fact(Skip = "Not a test. Scaffolds C# QuerySetup objects to file, read from a JSON seeded context")]
        public void ScaffoldCSharpQuerySetupsToFileUsingJsonSeededFacetContext()
        {
            var scaffolder = new DisposableFacetContextContainer(Fixture);

            // Uri format: "target-facet[@trigger-facet]:(facet-code[@picks])(/facet-code[@picks])*
            var uris = new List<string>() {
                "sites:sites:",
                "sites:sites@1",
                "sites:country@73/sites:",
                "tbl_denormalized_measured_values_33_0:tbl_denormalized_measured_values_33_0@(110,2904)"
            };
            var options = new DumpOptions()
            {
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

            foreach (var uri in uris)
            {
                var facetsConfig = scaffolder.FakeFacetsConfig(uri);
                var querySetup = scaffolder.FakeCountOrContentQuerySetup(facetsConfig);

                var path = Path.Join(DataFolder(), $"QuerySetup_{UriName(uri)}.cs.txt");
                ScaffoldUtility.Dump(querySetup, path, options);
            }
        }

        //[Theory(Skip = "Not a test. Scaffolds C# ResultConfigs objects to file")]
        //[InlineData("tabular", "site_level")]
        //[InlineData("tabular", "aggregate_all")]
        //[InlineData("tabular", "sample_group_level")]
        //[InlineData("map", "map_result")]
        //public void ScaffoldCSharpResultConfigsToFile(string viewTypeId, string resultKey)
        //{
        //    var resultConfig = ResultConfigFactory.Create(viewTypeId, resultKey);

        //    var path = Path.Join(DataFolder(), $"ResultConfig_{viewTypeId}_{resultKey}.cs.txt");

        //    ScaffoldUtility.Dump(resultConfig, path);
        //}
    }
}
