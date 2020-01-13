using System;
using System.Collections.Generic;
using System.Text;
using SeadQueryCore;

namespace SeadQueryTest.Infrastructure.Scaffolds
{
    public static class FacetsConfigInstances
    {

        //"tbl_denormalized_measured_values_33_0:tbl_denormalized_measured_values_33_0@(3,52)"

        public static Dictionary<string, FacetsConfig2> Store = new Dictionary<string, FacetsConfig2> {
            {
                "sites@sites:sites:",
                new FacetsConfig2 {
                    RequestId = "1",
                    Language = "",
                    RequestType = "populate",
                    TargetCode = "sites",
                    TargetFacet = FacetInstances.Store["sites"],
                    TriggerCode = "sites",
                    TriggerFacet = FacetInstances.Store["sites"],
                    FacetConfigs = new List<FacetConfig2>
                    {
                        new FacetConfig2 {
                            FacetCode = "sites",
                            Facet = FacetInstances.Store["sites"],
                            Position = 0,
                            Picks = new List<FacetConfigPick> { }
                        }
                    }
                }
            },
            {
                "sites@sites:country@73/sites:",
                new FacetsConfig2() {
                    RequestId = "1",
                    RequestType = "populate",
                    TargetCode = "sites",
                    TargetFacet = FacetInstances.Store["sites"],
                    TriggerCode = "sites",
                    TriggerFacet = FacetInstances.Store["sites"],
                    FacetConfigs = new List<FacetConfig2> {
                        new FacetConfig2() {
                            FacetCode = "country",
                            Facet = FacetInstances.Store["country"],
                            Position = 0,
                            Picks = new List<FacetConfigPick>
                            {
                                new FacetConfigPick(EPickType.discrete, 73M)
                            }
                        },
                        new FacetConfig2() {
                            FacetCode = "sites",
                            Facet = FacetInstances.Store["sites"],
                            Position = 1,
                            Picks = new List<FacetConfigPick>() { }
                        }
                    },
                    InactiveConfigs = null
                }
            },
            {
                "sites@sites:sites@1",
                new FacetsConfig2() {
                    RequestId = "1",
                    RequestType = "populate",
                    TargetCode = "sites",
                    TargetFacet = FacetInstances.Store["sites"],
                    TriggerCode = "sites",
                    TriggerFacet = FacetInstances.Store["sites"],
                    FacetConfigs = new List<FacetConfig2>() {
                        new FacetConfig2
                        {
                            FacetCode = "sites",
                            Facet = FacetInstances.Store["sites"],
                            Position = 0,
                            Picks = new List<FacetConfigPick>
                            {
                                new FacetConfigPick(EPickType.discrete, 1M)
                            },
                        }
                    }
                }
            },

            {  "tbl_denormalized_measured_values_33_0:tbl_denormalized_measured_values_33_0@(3,52)",
                new FacetsConfig2() {
                    RequestId = "1",
                    RequestType = "populate",
                    TargetCode = "tbl_denormalized_measured_values_33_0",
                    TargetFacet = FacetInstances.Store["tbl_denormalized_measured_values_33_0"],
                    TriggerCode = "tbl_denormalized_measured_values_33_0",
                    TriggerFacet = FacetInstances.Store["tbl_denormalized_measured_values_33_0"],
                    FacetConfigs = new List<FacetConfig2>() {
                        new FacetConfig2
                        {
                            FacetCode = "tbl_denormalized_measured_values_33_0",
                            Facet = FacetInstances.Store["tbl_denormalized_measured_values_33_0"],
                            Position = 0,
                            Picks = new List<FacetConfigPick>
                            {
                                new FacetConfigPick
                                {
                                    PickType = SeadQueryCore.EPickType.lower,
                                    PickValue = "3",
                                    Text = "3"
                                },
                                new FacetConfigPick
                                {
                                    PickType = SeadQueryCore.EPickType.upper,
                                    PickValue = "52",
                                    Text = "52"
                                }
                            }
                        }
                    },
                    InactiveConfigs = null
                }
            },

           {  "tbl_denormalized_measured_values_33_0:tbl_denormalized_measured_values_33_0@(110,2904)",
                new FacetsConfig2() {
                    RequestId = "2",
                    RequestType = "populate",
                    TargetCode = "tbl_denormalized_measured_values_33_0",
                    TargetFacet = FacetInstances.Store["tbl_denormalized_measured_values_33_0"],
                    TriggerCode = "tbl_denormalized_measured_values_33_0",
                    TriggerFacet = FacetInstances.Store["tbl_denormalized_measured_values_33_0"],
                    FacetConfigs = new List<FacetConfig2>() {
                        new FacetConfig2() {
                            FacetCode = "tbl_denormalized_measured_values_33_0",
                            Facet = FacetInstances.Store["tbl_denormalized_measured_values_33_0"],
                            Position = 1,
                            Picks = FacetConfigPick.CreateLowerUpper(110M, 2904M)
                        }
                    }
                }
            },

           {  "geochronology:geochronology@(370,293666)",
                new FacetsConfig2() {
                    RequestId = "8",
                    RequestType = "populate",
                    TargetCode = "geochronology",
                    TargetFacet = FacetInstances.Store["geochronology"],
                    TriggerCode = "geochronology",
                    TriggerFacet = FacetInstances.Store["geochronology"],
                    FacetConfigs = new List<FacetConfig2>() {
                        new FacetConfig2() {
                            FacetCode = "geochronology",
                            Facet = FacetInstances.Store["geochronology"],
                            Position = 1,
                            Picks = FacetConfigPick.CreateLowerUpper(370M, 293666M)
                        }
                    }
                }
            }
        };
    }
}
