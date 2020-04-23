using System.Collections.Generic;
using System.Linq;

namespace SQT.Mocks
{
    public class FacetConfigsByUriFixtures
    {
        public static List<string> Trail(params string[] trail)
        {
            return RouteFactory.ToPairs(trail.ToList());
        }


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
                }
            },

            new object[] {
                "country@sites:sites/country@73:",
                new List<List<string>> {
                    Trail("tbl_analysis_entities", "tbl_physical_samples", "tbl_sample_groups", "tbl_sites"),
                    Trail("tbl_analysis_entities", "tbl_datasets")
                }
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


    }
}
