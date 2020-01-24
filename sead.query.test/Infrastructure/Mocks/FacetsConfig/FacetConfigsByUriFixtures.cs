using System.Collections.Generic;
using System.Linq;

namespace SeadQueryTest.Mocks
{
    public class FacetConfigsByUriFixtures
    {
        public static List<string> Trail(params string[] trail)
        {
            return RouteFactory.ToPairs(trail.ToList());
        }

        // FIXME: Hardcoded counts can only be used for if fixed value data is used during testing
        public static Dictionary<string, int> __DiscreteFacetComputeCount = new Dictionary<string, int> {
             { "species", 4904 },
             { "tbl_biblio_modern", 3460 },
             { "relative_age_name", 388 },
             { "record_types", 19 },
             { "sample_groups", 2195 },
             { "sites", 1544 },
             { "country", 260 },
             { "ecocode", 158 },
             { "family", 529 },
             { "genus", 3951 },
             { "species_author", 3101 },
             { "feature_type", 41 },
             { "ecocode_system", 3 },
             //{ "abundance_classification", 0 },
             { "activeseason", 18 },
             { "tbl_biblio_sample_groups", 2344 }
            };

        public Dictionary<string, int> DiscreteFacetComputeCount { get { return __DiscreteFacetComputeCount; } }

        public List<object[]> DiscreteTestConfigsWithoutPicks = new List<object[]> {

            /* No picks */
           new object[] {
                "sites:sites",
                new List<List<string>> {
                    Trail("tbl_analysis_entities", "tbl_physical_samples", "tbl_sample_groups", "tbl_sites"),
                    Trail("tbl_analysis_entities", "tbl_datasets")
                }
           },
           new object[] {
                "country:country",
                new List<List<string>> {
                    Trail("tbl_analysis_entities", "tbl_physical_samples", "tbl_sample_groups", "tbl_sites", "tbl_site_locations"),
                    Trail("tbl_site_locations", "countries"),
                    Trail("tbl_analysis_entities", "tbl_datasets")
                }
           },
           new object[] {
                "ecocode:sites/ecocode",
                new List<List<string>> {
                    Trail("tbl_analysis_entities", "tbl_abundances", "tbl_taxa_tree_master", "tbl_ecocodes", "tbl_ecocode_definitions"),
                    Trail("tbl_analysis_entities", "tbl_physical_samples"),
                    Trail("tbl_analysis_entities", "tbl_datasets")
                }
           },
           new object[] {
                "country:sites/country",
                new List<List<string>> {
                    Trail("tbl_analysis_entities", "tbl_physical_samples", "tbl_sample_groups", "tbl_sites", "tbl_site_locations"),
                    Trail("tbl_site_locations", "countries"),
                    Trail("tbl_analysis_entities", "tbl_datasets")
                }
           },
           new object[] {
                "ecocode:country/sites/ecocode",
                new List<List<string>> {
                    Trail("tbl_analysis_entities", "tbl_abundances", "tbl_taxa_tree_master", "tbl_ecocodes", "tbl_ecocode_definitions"),
                    Trail("tbl_analysis_entities", "tbl_physical_samples"),
                    Trail("tbl_analysis_entities", "tbl_datasets")
                }
           },
           new object[] {
                "sites:country/sites/ecocode",
                new List<List<string>> {
                    Trail("tbl_analysis_entities", "tbl_abundances", "tbl_taxa_tree_master", "tbl_ecocodes", "tbl_ecocode_definitions"),
                    Trail("tbl_analysis_entities", "tbl_physical_samples"),
                    Trail("tbl_analysis_entities", "tbl_datasets")
                }
           }
        };

        public List<object[]> DiscreteTestConfigsWithPicks = new List<object[]> {

            new object[] {
                "country@country:sites/country@73:",
                new List<List<string>> {
                    Trail("tbl_analysis_entities", "tbl_physical_samples", "tbl_sample_groups", "tbl_sites"),
                    Trail("tbl_analysis_entities", "tbl_datasets")
                },
                __DiscreteFacetComputeCount["country"]
            },

            new object[] {
                "country@sites:sites/country@73:",
                new List<List<string>> {
                    Trail("tbl_analysis_entities", "tbl_physical_samples", "tbl_sample_groups", "tbl_sites"),
                    Trail("tbl_analysis_entities", "tbl_datasets")
                },
                __DiscreteFacetComputeCount["country"]
            },

            new object[] {
                "sites@sites:country@73/sites:",
                new List<List<string>> {
                    Trail("tbl_analysis_entities", "tbl_physical_samples", "tbl_sample_groups", "tbl_sites"),
                    Trail("tbl_analysis_entities", "tbl_datasets")
                },
                30
            },

            new object[] {
                "sites@country:country@73/sites:",
                new List<List<string>> {
                    Trail("tbl_analysis_entities", "tbl_physical_samples", "tbl_sample_groups", "tbl_sites"),
                    Trail("tbl_analysis_entities", "tbl_datasets")
                },
                30
            }

        };

        public List<object[]> RangeTestConfigsWithoutPicks = new List<object[]> {

            /* MS */
            new object[] {
                "tbl_denormalized_measured_values_33_0:tbl_denormalized_measured_values_33_0:",
                new List<List<string>> {
                    Trail("metainformation.tbl_denormalized_measured_values", "tbl_physical_samples", "tbl_analysis_entities")
                },
                11
            }

        };

        public Dictionary<string,int> FinishSiteCount = new Dictionary<string, int>() {
                    #region __data__
                    { "1029", 1 },
                        { "1522", 1 },
                        { "1510", 1 },
                        { "1521", 1 },
                        { "1525", 1 },
                        { "1512", 1 },
                        { "1209", 1 },
                        { "1210", 1 },
                        { "1516", 1 },
                        { "1200", 1 },
                        { "1201", 1 },
                        { "1054", 3 },
                        { "1517", 1 },
                        { "1518", 1 },
                        { "742", 4 },
                        { "1519", 1 },
                        { "1523", 1 },
                        { "1515", 1 },
                        { "1026", 1 },
                        { "1511", 1 },
                        { "1028", 1 },
                        { "1526", 1 },
                        { "1520", 1 },
                        { "1208", 1 },
                        { "1527", 1 },
                        { "1205", 1 },
                        { "1528", 1 },
                        { "1514", 1 },
                        { "1513", 1 },
                        { "1524", 1 }
                    };
        #endregion

    }
}
