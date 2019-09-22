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
                    ReducedRoutes = new List<GraphRoute> { },
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
                            Items = new List<GraphEdge>
                            {
                                new GraphEdge
                                {
                                    EdgeId = -1140,
                                    SourceNodeId = 119,
                                    TargetNodeId = 113,
                                    Weight = 20,
                                    SourceKeyName = "site_id",
                                    TargetKeyName = "site_id",
                                    SourceNode = new GraphNode
                                    {
                                        NodeId = 119,
                                        TableName = "tbl_sites"
                                    },
                                    TargetNode = new GraphNode
                                    {
                                        NodeId = 113,
                                        TableName = "tbl_site_locations"
                                    }
                                }
                            }
                        },
                        new GraphRoute
                        {
                            Items = new List<GraphEdge>
                            {
                                new GraphEdge
                                {
                                    EdgeId = -1140,
                                    SourceNodeId = 119,
                                    TargetNodeId = 113,
                                    Weight = 20,
                                    SourceKeyName = "site_id",
                                    TargetKeyName = "site_id",
                                    SourceNode = new GraphNode
                                    {
                                        NodeId = 119,
                                        TableName = "tbl_sites"
                                    },
                                    TargetNode = new GraphNode
                                    {
                                        NodeId = 113,
                                        TableName = "tbl_site_locations"
                                    }
                                },
                                new GraphEdge
                                {
                                    EdgeId = 1151,
                                    SourceNodeId = 113,
                                    TargetNodeId = 46,
                                    Weight = 5,
                                    SourceKeyName = "location_id",
                                    TargetKeyName = "location_id",
                                    SourceNode = new GraphNode
                                    {
                                        NodeId = 113,
                                        TableName = "tbl_site_locations"
                                    },
                                    TargetNode = new GraphNode
                                    {
                                        NodeId = 46,
                                        TableName = "countries"
                                    }
                                }
                            }
                        }
                    },
                    ReducedRoutes = new List<GraphRoute>
                    {
                        new GraphRoute
                        {
                            Items = new List<GraphEdge>
                            {
                                new GraphEdge
                                {
                                    EdgeId = -1140,
                                    SourceNodeId = 119,
                                    TargetNodeId = 113,
                                    Weight = 20,
                                    SourceKeyName = "site_id",
                                    TargetKeyName = "site_id",
                                    SourceNode = new GraphNode
                                    {
                                        NodeId = 119,
                                        TableName = "tbl_sites"
                                    },
                                    TargetNode = new GraphNode
                                    {
                                        NodeId = 113,
                                        TableName = "tbl_site_locations"
                                    }
                                }
                            }
                        },
                        new GraphRoute
                        {
                            Items = new List<GraphEdge>
                            {
                                new GraphEdge
                                {
                                    EdgeId = 1151,
                                    SourceNodeId = 113,
                                    TargetNodeId = 46,
                                    Weight = 5,
                                    SourceKeyName = "location_id",
                                    TargetKeyName = "location_id",
                                    SourceNode = new GraphNode
                                    {
                                        NodeId = 113,
                                        TableName = "tbl_site_locations"
                                    },
                                    TargetNode = new GraphNode
                                    {
                                        NodeId = 46,
                                        TableName = "countries"
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
