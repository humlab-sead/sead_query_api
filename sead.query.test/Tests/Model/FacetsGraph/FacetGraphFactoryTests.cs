using Moq;
using SeadQueryCore;
using SeadQueryInfra;
using SeadQueryTest.Infrastructure.Scaffolding;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace SeadQueryTest.Model
{
    public class FacetGraphFactoryTests
    {
        private readonly RepositoryRegistry mockRegistry;

        public FacetGraphFactoryTests()
        {
            mockRegistry = new RepositoryRegistry(ScaffoldUtility.JsonSeededFacetContext());
        }

        private IRepositoryRegistry CreateMockRegistry()
        {
            var nodes = new List<Table>() {
                new Table { TableId = 0, TableOrUdfName = "A" },
                new Table { TableId = 0, TableOrUdfName = "B" },
                new Table { TableId = 0, TableOrUdfName = "C" },
                new Table { TableId = 0, TableOrUdfName = "D" },
                new Table { TableId = 0, TableOrUdfName = "E" },
                new Table { TableId = 0, TableOrUdfName = "F" },
                new Table { TableId = 0, TableOrUdfName = "G" },
                new Table { TableId = 0, TableOrUdfName = "H" }
            };
            var nodesDict = nodes.ToDictionary(z => z.TableOrUdfName);
            var edges = new List<TableRelation>() {
                new TableRelation() { TableRelationId = 0, SourceTable = nodesDict["A"], TargetTable = nodesDict["B"], Weight = 7 },
                new TableRelation() { TableRelationId = 0, SourceTable = nodesDict["A"], TargetTable = nodesDict["C"], Weight = 8 },
                new TableRelation() { TableRelationId = 0, SourceTable = nodesDict["B"], TargetTable = nodesDict["F"], Weight = 2 },
                new TableRelation() { TableRelationId = 0, SourceTable = nodesDict["C"], TargetTable = nodesDict["F"], Weight = 6 },
                new TableRelation() { TableRelationId = 0, SourceTable = nodesDict["C"], TargetTable = nodesDict["G"], Weight = 4 },
                new TableRelation() { TableRelationId = 0, SourceTable = nodesDict["D"], TargetTable = nodesDict["F"], Weight = 8 },
                new TableRelation() { TableRelationId = 0, SourceTable = nodesDict["E"], TargetTable = nodesDict["H"], Weight = 1 },
                new TableRelation() { TableRelationId = 0, SourceTable = nodesDict["F"], TargetTable = nodesDict["G"], Weight = 9 },
                new TableRelation() { TableRelationId = 0, SourceTable = nodesDict["F"], TargetTable = nodesDict["H"], Weight = 3 }
            };
            var mockRegistry = new Mock<IRepositoryRegistry>();
            mockRegistry.Setup(x => x.Tables.GetAll()).Returns(nodes);
            mockRegistry.Setup(x => x.TableRelations.GetAll()).Returns(edges);
            mockRegistry.Setup(x => x.FacetTables.AliasTablesDict()).Returns(new Dictionary<string, FacetTable>());
            return mockRegistry.Object;
        }

        private FacetGraphFactory CreateFactory()
        {
            IRepositoryRegistry mockRegistry = CreateMockRegistry();
            return new FacetGraphFactory(mockRegistry);
        }

        [Fact]
        public void Build_WhenSuccessfullyCalled_HasExpectedNodesAndEdges()
        {
            List<string> nodeData = new List<string> { "A", "B", "C", "D", "E", "F", "G", "H" };
            List<(string, string, int)> edgeData = new List<(string, string, int)> {
                ("E", "H", 1),
                ("F", "G", 9),
                ("F", "H", 3),
            };

            //List<Table> nodes = Enumerable.Range(0, nodeData.Count)
            //    .Select(i => new Table() { TableId = i + 1, TableOrUdfName = nodeData[i] })
            //    .ToList();

            //var nodesDict = nodes.ToDictionary(n => n.TableOrUdfName);

            //// Arrange

            //List<TableRelation> edges = Enumerable.Range(0, edgeData.Count)
            //    .Select(
            //        i =>
            //            new TableRelation() {
            //                TableRelationId = i + 1,
            //                SourceTable = nodesDict[edgeData[i].Item1],
            //                TargetTable = nodesDict[edgeData[i].Item2],
            //                Weight = edgeData[i].Item3,
            //                SourceColumName = "",
            //                TargetColumnName = ""
            //            }
            //    ).ToList();

            List<Facet> aliasFacets = new List<Facet>();

            var factory = this.CreateFactory();

            // Act
            var result = factory.Build();

            // Assert
            Assert.Equal(nodeData.Count, result.Nodes.Count);
            Assert.Equal(nodeData.Count, result.NodesIds.Count);
            Assert.Equal(2 * edgeData.Count, result.Edges.Count);
            //Assert.Collection(nodeData, x => result.Nodes.ContainsKey(x));
            //Assert.Collection(edgeData, x => result.GetEdge(x.Item1, x.Item2));
        }
    }
}
