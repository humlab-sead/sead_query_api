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
    public class FacetGraphFactoryTests : DisposableFacetContextContainer
    {
        public FacetGraphFactoryTests(JsonSeededFacetContextFixture fixture) : base(fixture)
        {
        }

        private FacetGraphFactory CreateFactory()
        {
            return SimpleFacetGraphFactoryFactory.Create();
        }

        [Fact]
        public void Build_WhenSuccessfullyCalled_HasExpectedNodesAndEdges()
        {

            List<Facet> aliasFacets = new List<Facet>();

            var factory = this.CreateFactory();

            // Act
            var result = factory.Build();

            // Assert
            Assert.Equal(SimpleFacetGraphRepositoryRegistryFactory.Nodes.Count, result.Nodes.Count);
            Assert.Equal(SimpleFacetGraphRepositoryRegistryFactory.Nodes.Count, result.NodesIds.Count);
            Assert.Equal(2 * SimpleFacetGraphRepositoryRegistryFactory.Edges.Count, result.Edges.Count);

        }
    }
}
