using Moq;
using SeadQueryCore;
using System;
using System.Collections.Generic;
using Xunit;

namespace SeadQueryTest.Model.Entities
{
    public class GraphRouteTests : IDisposable
    {
        private Moq.MockRepository mockRepository;

        private Mock<List<GraphEdge>> mockList;

        public GraphRouteTests()
        {
            this.mockRepository = new Moq.MockRepository(MockBehavior.Strict);

            this.mockList = this.mockRepository.Create<List<GraphEdge>>();
        }

        public void Dispose()
        {
            this.mockRepository.VerifyAll();
        }

        private GraphRoute CreateGraphRoute()
        {
            return new GraphRoute(
                this.mockList.Object);
        }

        [Fact]
        public void Contains_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var graphRoute = this.CreateGraphRoute();
            GraphEdge item = null;

            // Act
            var result = graphRoute.Contains(
                item);

            // Assert
            Assert.True(false);
        }

        [Fact]
        public void ReduceBy_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var graphRoute = this.CreateGraphRoute();
            List<GraphRoute> routes = null;

            // Act
            var result = graphRoute.ReduceBy(
                routes);

            // Assert
            Assert.True(false);
        }

        [Fact]
        public void ToString_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var graphRoute = this.CreateGraphRoute();

            // Act
            var result = graphRoute.ToString();

            // Assert
            Assert.True(false);
        }

        [Fact]
        public void Trail_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var graphRoute = this.CreateGraphRoute();

            // Act
            var result = graphRoute.Trail();

            // Assert
            Assert.True(false);
        }
    }
}
