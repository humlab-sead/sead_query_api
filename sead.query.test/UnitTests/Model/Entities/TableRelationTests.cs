using SeadQueryCore;
using SQT.Infrastructure;
using SQT.Mocks;
using System;
using System.Collections.Generic;
using Xunit;

namespace SQT.Model;

using Graph = List<TableRelation>;
using Route = List<TableRelation>;

public class TableRelationTests
{
    public Graph Graph { get; private set; }
    public Route Route { get; private set; }
    public List<Route> Routes { get; private set; }

    public TableRelationTests()
    {
        Graph = FakeGraphFactory.CreateSimpleGraph();
        Route = [
                Graph.GetEdge("A", "B"),
                Graph.GetEdge("B", "F"),
                Graph.GetEdge("F", "H")
            ];
        Routes = [
            Route,
            [
                Graph.GetEdge("F", "B"),
                Graph.GetEdge("B", "A"),
                Graph.GetEdge("A", "C")
            ]];
    }

    [Fact]
    public void GetEdge_WithStringParameters_ReturnsCorrectEdge()
    {
        var edge = Graph.GetEdge("A", "B");

        Assert.NotNull(edge);
        Assert.Equal("A", edge.SourceName);
        Assert.Equal("B", edge.TargetName);

        edge = Route.GetEdge("A", "B");

        Assert.NotNull(edge);
        Assert.Equal("A", edge.SourceName);
        Assert.Equal("B", edge.TargetName);
    }

    [Fact]
    public void GetEdge_WithIntParameters_ReturnsCorrectEdge()
    {
        var AB = Graph.GetEdge("A", "B");

        var edge = Graph.GetEdge(AB.SourceTable.TableId, AB.TargetTable.TableId);

        Assert.NotNull(edge);

        Assert.Equal("A", edge.SourceName);
        Assert.Equal("B", edge.TargetName);
    }

    [Fact]
    public void GetEdge_WithUnknownEdge_ReturnsNull()
    {
        var edge = Graph.GetEdge("A", "Z");

        Assert.Null(edge);
    }

    [Fact]
    public void HasEdge_WithUnknownEdge_ReturnsFalse()
    {
        Assert.True(Graph.HasEdge(Graph.GetEdge("A", "B")));
        Assert.True(Graph.HasEdge("A", "C"));
        Assert.False(Route.HasEdge("A", "C"));
        Assert.False(Graph.HasEdge("A", "Z"));
    }


    [Fact]
    public void ReduceEdges_Route_ItemsRemoved()
    {
        Route route = [
                Graph.GetEdge("A", "B"),
                Graph.GetEdge("B", "F"),
                Graph.GetEdge("F", "H")
            ];

        var result = route.ReduceEdges([
            [
                Graph.GetEdge("F", "H")
            ]
        ]);

        Assert.Equal(2, result.Count);
        Assert.True(route.HasEdge(Graph.GetEdge("A", "B")));
        Assert.True(route.HasEdge(Graph.GetEdge("B", "F")));

        result = route.ReduceEdges([
            [
                Graph.GetEdge("A", "B"),
                Graph.GetEdge("B", "F")
            ]
        ]);

        Assert.Single(result);
        Assert.True(route.HasEdge(Graph.GetEdge("F", "H")));

        result = route.ReduceEdges([
            [
                Graph.GetEdge("A", "B"),
            ],
            [
                Graph.GetEdge("B", "F")
            ]
        ]);

        Assert.Single(result);
        Assert.True(route.HasEdge(Graph.GetEdge("F", "H")));

    }

    [Fact]
    public void ReduceEdges_Routes_ItemsRemoved()
    {
        List<Route> routes = [
            [
                Graph.GetEdge("F", "H")
            ],
            [
                Graph.GetEdge("A", "B"),
                Graph.GetEdge("B", "F"),
                Graph.GetEdge("F", "H")
            ]
        ];

        var result = routes.ReduceEdges();

        Assert.Equal(2, result.Count);
        Assert.Single(result[0]);
        Assert.True(result[0].HasEdge(Graph.GetEdge("F", "H")));

        Assert.Equal(2, result[1].Count);

        Assert.True(result[1].HasEdge(Graph.GetEdge("A", "B")));
        Assert.True(result[1].HasEdge(Graph.GetEdge("B", "F")));

        Assert.Empty(new List<Route>().ReduceEdges());

    }

    [Fact]
    public void GetFlattenEdges_ReturnsFlattenedAndOrderedEdges()
    {
        // Arrange
        var routes = new List<Route>
        {
            new Route
            {
                new TableRelation { TargetTable = new Table { IsUdf = false } },
                new TableRelation { TargetTable = new Table { IsUdf = true } }
            },
            new Route
            {
                new TableRelation { TargetTable = new Table { IsUdf = true } },
                new TableRelation { TargetTable = new Table { IsUdf = false } }
            }
        };

        // Act
        var result = routes.GetFlattenEdges();

        // Assert
        Assert.Equal(4, result.Count);
        Assert.True(result[0].TargetTable.IsUdf);
        Assert.False(result[3].TargetTable.IsUdf);
    }

    [Fact]
    public void ToTuples_ConvertsRouteToTuplesCorrectly()
    {
        var AB = Graph.GetEdge("A", "B");
        var BF = Graph.GetEdge("B", "F");

        var edges = new Route { AB, BF };

        var A = AB.SourceTable;
        var B = AB.TargetTable;
        var F = BF.TargetTable;

        var result = edges.ToTuples();

        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
        Assert.Equal(Tuple.Create(A.TableId, B.TableId, AB.Weight), result[0]);
        Assert.Equal(Tuple.Create(B.TableId, F.TableId, BF.Weight), result[1]);
    }

    [Fact]
    public void ToValueTuples_ConvertsRouteToValueTuplesCorrectly()
    {
        var AB = Graph.GetEdge("A", "B");
        var BF = Graph.GetEdge("B", "F");

        var edges = new Route { AB, BF };

        var A = AB.SourceTable;
        var B = AB.TargetTable;
        var F = BF.TargetTable;

        var result = edges.ToValueTuples();

        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
        Assert.Equal((A.TableId, B.TableId, AB.Weight), result[0]);
        Assert.Equal((B.TableId, F.TableId, BF.Weight), result[1]);
    }

    [Fact]
    public void ToString_ReturnsCorrectStringRepresentationOfRoute()
    {
        var AB = Graph.GetEdge("A", "B");
        var BF = Graph.GetEdge("B", "F");

        var route = new Route { AB, BF };

        var result = route.ToEdgeString();

        var expected = $"A;B;{AB.Weight}\nB;F;{BF.Weight}";

        Assert.Equal(expected, result);
    }

    [Fact]
    public void ToString_ReturnsCorrectStringRepresentationOfRoutes()
    {
        var AB = Graph.GetEdge("A", "B");
        var BF = Graph.GetEdge("B", "F");

        var routes = new List<Route>() { new Route() { AB }, new Route() { BF } };

        var result = routes.ToEdgeString();

        var expected = $"A;B;{AB.Weight}\nB;F;{BF.Weight}";

        Assert.Equal(expected, result);
    }


    [Fact]
    public void ToString_OfAnyGraph_ReturnsCsvString()
    {
        // Arrange
        var route = Route;
        const string expected = "A;B;7\nB;F;2\nF;H;3";
        // Act
        var result = route.ToEdgeString();

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void Trail_OfAnyGraph_ReturnListOfNodesNamesInTrail()
    {
        // Arrange
        var route = Route;
        var expected = "A-B-F-H".Split('-');

        // Act
        var result = route.ToTrail();

        // Assert
        Assert.Equal(expected, result);

    }

    [Fact]
    public void Equals_OfIdenticalEdges_IsTrue()
    {
        Assert.True(Graph.GetEdge("A", "B").Equals(Graph.GetEdge("A", "B")));
    }

    [Fact]
    public void Equals_OfNonIdenticalEdges_IsFalse()
    {
        Assert.False(Graph.GetEdge("A", "B").Equals(Graph.GetEdge("F", "H")));
    }


    [Fact]
    public void Clone_OfAnyEdge_HasSameState()
    {
        var AB = Graph.GetEdge("A", "B");

        var AB_copy = AB.Clone();

        Asserter.EqualByProperty(AB, AB_copy);
    }

    [Fact]
    public void Reverse_OfAnyGraph_SwitchesNodes()
    {
        var AB = Graph.GetEdge("A", "B");

        var BA = AB.Reverse();

        // Assert
        Assert.Equal(BA.SourceName, AB.TargetName);
        Assert.Equal(BA.SourceTable, AB.TargetTable);
        Assert.Equal(BA.TargetName, AB.SourceName);
        Assert.Equal(BA.TargetTable, AB.SourceTable);
        Assert.Equal(BA.SourceColumName, AB.TargetColumnName);
        Assert.Equal(BA.TargetColumnName, AB.SourceColumName);
        Assert.Equal(BA.Weight, AB.Weight);
    }

    [Fact]
    public void Alias_OfAnyGraph_ReplacesNodeWithSameId()
    {
        // Arrange
        var AB = Graph.GetEdge("A", "B");

        Table node = AB.TargetTable;
        Table alias = new Table { TableId = node.TableId, TableOrUdfName = "ALIAS" };

        // Act
        var result = AB.Alias(node, alias);

        // Assert
        Assert.Equal("ALIAS", result.TargetTable.TableOrUdfName);
    }


}
