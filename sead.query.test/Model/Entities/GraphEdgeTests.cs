using Moq;
using SeadQueryCore;
using System;
using Xunit;

namespace SeadQueryTest.Model.Entities
{
    public class GraphEdgeTests : IDisposable
    {
        private Moq.MockRepository mockRepository;



        public GraphEdgeTests()
        {
            this.mockRepository = new Moq.MockRepository(MockBehavior.Strict);


        }

        public void Dispose()
        {
            this.mockRepository.VerifyAll();
        }

        private GraphEdge CreateGraphEdge()
        {
            return new GraphEdge();
        }

        [Fact]
        public void Clone_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var graphEdge = this.CreateGraphEdge();

            // Act
            var result = graphEdge.Clone();

            // Assert
            Assert.True(false);
        }

        [Fact]
        public void Reverse_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var graphEdge = this.CreateGraphEdge();

            // Act
            var result = graphEdge.Reverse();

            // Assert
            Assert.True(false);
        }

        [Fact]
        public void Alias_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var graphEdge = this.CreateGraphEdge();
            GraphNode node = null;
            GraphNode alias = null;

            // Act
            var result = graphEdge.Alias(
                node,
                alias);

            // Assert
            Assert.True(false);
        }

        [Fact]
        public void Equals_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var graphEdge = this.CreateGraphEdge();
            object x = null;

            // Act
            var result = graphEdge.Equals(
                x);

            // Assert
            Assert.True(false);
        }

        [Fact]
        public void Equal_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var graphEdge = this.CreateGraphEdge();
            GraphEdge x = null;

            // Act
            var result = graphEdge.Equal(
                x);

            // Assert
            Assert.True(false);
        }

        [Fact]
        public void GetHashCode_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var graphEdge = this.CreateGraphEdge();

            // Act
            var result = graphEdge.GetHashCode();

            // Assert
            Assert.True(false);
        }

        [Fact]
        public void ToStringPair_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var graphEdge = this.CreateGraphEdge();

            // Act
            var result = graphEdge.ToStringPair();

            // Assert
            Assert.True(false);
        }
    }
}
