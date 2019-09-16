using Moq;
using SeadQueryCore;
using System;
using System.Collections.Generic;
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
        public void Build_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var factory = this.CreateFactory();
            List<GraphNode> nodes = null;
            List<GraphEdge> edges = null;
            List<Facet> aliasFacets = null;

            // Act
            var result = factory.Build(
                nodes,
                edges,
                aliasFacets);

            // Assert
            Assert.True(false);
        }
    }
}
