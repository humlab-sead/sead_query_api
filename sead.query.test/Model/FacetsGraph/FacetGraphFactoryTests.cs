using Moq;
using SeadQueryCore;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace SeadQueryTest.Model
{
    public class FacetGraphFactoryTests : IDisposable
    {
        private Moq.MockRepository mockRepository;
        public FacetGraphFactoryTests()
        {
            this.mockRepository = new Moq.MockRepository(MockBehavior.Strict);
        }

        public void Dispose()
        {
            this.mockRepository.VerifyAll();
        }

        private FacetGraphFactory CreateFactory()
        {
            return new FacetGraphFactory();
        }

        [Fact]
        public void Build_WhenSuccessfullyCalled_HasExpectedNodesAndEdges()
        {
            List<string> nodeData = new List<string> { "A", "B", "C", "D", "E", "F", "G", "H" };
            List<(string, string, int)> edgeData = new List<(string, string, int)> {
                ("A", "B", 7),
                ("A", "C", 8),
                ("B", "F", 2),
                ("C", "F", 6),
                ("C", "G", 4),
                ("D", "F", 8),
                ("E", "H", 1),
                ("F", "G", 9),
                ("F", "H", 3),
            };

            List<GraphNode> nodes = Enumerable.Range(0, nodeData.Count)
                .Select(i => new GraphNode() { NodeId = i + 1, TableName = nodeData[i] })
                .ToList();

            var nodesDict = nodes.ToDictionary(n => n.TableName);

            // Arrange

            List<GraphEdge> edges = Enumerable.Range(0, edgeData.Count)
                .Select(
                    i => 
                        new GraphEdge() {
                            EdgeId = i + 1,
                            SourceNode = nodesDict[edgeData[i].Item1],
                            TargetNode = nodesDict[edgeData[i].Item2],
                            Weight = edgeData[i].Item3,
                            SourceKeyName = "",
                            TargetKeyName = ""
                        }
                ).ToList();

            List<Facet> aliasFacets = new List<Facet>();

            var factory = this.CreateFactory();

            // Act
            var result = factory.Build(
                nodes,
                edges,
                aliasFacets
            );

            // Assert
            Assert.Equal(nodeData.Count, result.Nodes.Count);
            Assert.Equal(nodeData.Count, result.NodesIds.Count);
            Assert.Equal(2 * edgeData.Count, result.Edges.Count);
            //Assert.Collection(nodeData, x => result.Nodes.ContainsKey(x));
            //Assert.Collection(edgeData, x => result.GetEdge(x.Item1, x.Item2));
        }
    }
}
