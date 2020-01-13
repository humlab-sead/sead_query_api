using System;
using System.Collections.Generic;
using SeadQueryCore;
using SeadQueryCore.QueryBuilder;
using SeadQueryTest.Fixtures;

namespace SeadQueryTest.Infrastructure.Scaffolds
{
    public static class QuerySetupInstances
    {
        public static Dictionary<string, QuerySetup> Store = new Dictionary<string, QuerySetup> {
            {
                "tbl_denormalized_measured_values_33_0:tbl_denormalized_measured_values_33_0@(110,2904)",
                new QuerySetup {
                    TargetConfig = new FacetConfig2
                    {
                        FacetCode = "tbl_denormalized_measured_values_33_0",
                        Facet = FacetInstances.Store["tbl_denormalized_measured_values_33_0"],
                        Position = 0,
                        TextFilter = "",
                        Picks = FacetConfigPick.CreateLowerUpper(110M, 2904M)
                    },
                    Routes = new List<GraphRoute> { },
                    Joins = new List<String> { },
                    Criterias = new List<String>
                    {
                        "(( (method_values.measured_value >= 110 and method_values.measured_value <= 2904)))"
                    }
                }
            },
            {
                "sites@sites:country@73/sites:",
                new QuerySetup {
                    TargetConfig = new FacetConfig2
                    {
                        FacetCode = "sites",
                        Facet = FacetInstances.Store["sites"],
                        Position = 1,
                        TextFilter = "",
                        Picks = new List<FacetConfigPick>
                        {
                        }
                    },
                    Routes = new List<GraphRoute>
                    {
                        new GraphRoute
                        {
                            Items = new List<TableRelation>
                            {
                                new TableRelation
                                {
                                    TableRelationId = -1140,
                                    SourceTableId = 119,
                                    TargetTableId = 113,
                                    Weight = 20,
                                    SourceColumName = "site_id",
                                    TargetColumnName = "site_id",
                                    SourceTable = new Table
                                    {
                                        TableId = 119,
                                        TableOrUdfName = "tbl_sites"
                                    },
                                    TargetTable = new Table
                                    {
                                        TableId = 113,
                                        TableOrUdfName = "tbl_site_locations"
                                    }
                                }
                            }
                        },
                        new GraphRoute
                        {
                            Items = new List<TableRelation>
                            {
                                new TableRelation
                                {
                                    TableRelationId = 1151,
                                    SourceTableId = 113,
                                    TargetTableId = 46,
                                    Weight = 5,
                                    SourceColumName = "location_id",
                                    TargetColumnName = "location_id",
                                    SourceTable = new Table
                                    {
                                        TableId = 113,
                                        TableOrUdfName = "tbl_site_locations"
                                    },
                                    TargetTable = new Table
                                    {
                                        TableId = 46,
                                        TableOrUdfName = "countries"
                                    }
                                }
                            }
                        }
                    },
                    Joins = new List<String>
                    {
                        " left join tbl_site_locations  on tbl_site_locations.\"site_id\" = tbl_sites.\"site_id\" ",
                        " inner join tbl_locations countries on countries.\"location_id\" = tbl_site_locations.\"location_id\" "
                    },
                    Criterias = new List<String>
                    {
                        "(( (countries.location_id::int in (73)) ))"
                    }
                }
            }
        };
    }
}
