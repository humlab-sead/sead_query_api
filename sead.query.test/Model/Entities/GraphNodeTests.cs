using Moq;
using SeadQueryCore;
using System;
using Xunit;

namespace SeadQueryTest.Model.Entities
{
    public class GraphNodeTests : IDisposable
    {
        private Moq.MockRepository mockRepository;



        public GraphNodeTests()
        {
            this.mockRepository = new Moq.MockRepository(MockBehavior.Strict);


        }

        public void Dispose()
        {
            this.mockRepository.VerifyAll();
        }

        private GraphNode CreateGraphNode()
        {
            return new GraphNode();
        }

        [Fact]
        public void Equals_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var graphNode = this.CreateGraphNode();
            object obj = null;

            // Act
            var result = graphNode.Equals(
                obj);

            // Assert
            Assert.True(false);
        }

        [Fact]
        public void Equals_StateUnderTest_ExpectedBehavior1()
        {
            // Arrange
            var graphNode = this.CreateGraphNode();
            GraphNode obj = null;

            // Act
            var result = graphNode.Equals(
                obj);

            // Assert
            Assert.True(false);
        }

        [Fact]
        public void GetHashCode_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var graphNode = this.CreateGraphNode();

            // Act
            var result = graphNode.GetHashCode();

            // Assert
            Assert.True(false);
        }
    }
}
