using Moq;
using SeadQueryCore;
using SeadQueryInfra;
using SeadQueryTest.Mocks;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using SeadQueryTest.Infrastructure;

namespace SeadQueryTest.Model
{
    public class FacetGraphFactoryTests : IDisposable
    {
        private readonly FacetContext mockContext;
        private readonly RepositoryRegistry mockRegistry;

        public FacetGraphFactoryTests()
        {
            mockContext = JsonSeededFacetContextFactory.Create();
            mockRegistry = new RepositoryRegistry(mockContext);
        }

        public void Dispose()
        {
            mockContext.Dispose();
            mockRegistry.Dispose();
        }

        private FacetGraphFactory CreateFactory()
        {
            return SimpleFacetGraphFactoryFactory.Create();
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

            List<Facet> aliasFacets = new List<Facet>();

            var factory = this.CreateFactory();

            // Act
            var result = factory.Build();

            // Assert
            Assert.Equal(nodeData.Count, result.Nodes.Count);
            Assert.Equal(nodeData.Count, result.NodesIds.Count);
            Assert.Equal(2 * edgeData.Count, result.Edges.Count);

        }
    }
}
