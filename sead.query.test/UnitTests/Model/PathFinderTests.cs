using Moq;
using SeadQueryCore;
using System;
using System.Collections.Generic;
using Xunit;
using Autofac;
using SQT.Infrastructure;
using SeadQueryInfra;
using SQT.Mocks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.Sqlite;
using System.Threading.Tasks;
using SQT.Scaffolding;

namespace SQT.Model
{
    using Route = List<TableRelation>;

    [Collection("UsePostgresFixture")]
    public class RouteFinderTests : MockerWithFacetContext
    {
        public RouteFinderTests() : base()
        {
        }

        private IPathFinder CreateFacetsGraphByFakeContext(IFacetContext facetContext)
        {
            return ScaffoldUtility.DefaultRouteFinder(facetContext);
        }

        private IContainer CreateDependencyContainer()
        {
            var container = DependencyService.CreateContainer(FacetContext, MockSettings().Object);
            return container;
        }

        //[Fact]
        //public void IsAlias_WhenGraphHasAlias_ShouldBeTrue()
        //{
        //    // Arrange
        //    var finder = MockFacetGraphGenerator.CreateSimpleGraph();

        //    const string name = "X";

        //    // Act
        //    var result = finder.IsAlias(name);

        //    // Assert
        //    Assert.True(result);
        //}

        //[Fact]
        //public void ResolveTargetName_WhenGraphHasAlias_ShouldBeTargetName()
        //{
        //    // Arrange
        //    var finder = MockFacetGraphGenerator.CreateSimpleGraph();

        //    const string aliasOrTable = "X";

        //    // Act
        //    var result = finder.ResolveTargetName(
        //        aliasOrTable);

        //    // Assert
        //    Assert.Equal("A", result);
        //}

        //[Fact]
        //public void ResolveAliasName_WhenGraphHasAlias_ShouldBeAliasName()
        //{
        //    // Arrange
        //    var finder = MockFacetGraphGenerator.CreateSimpleGraph();

        //    const string aliasOrTable = "X";

        //    // Act
        //    var result = finder.ResolveAliasName(aliasOrTable);

        //    // Assert
        //    Assert.Equal("X", result);
        //}

        [Fact]
        public void Build_WhenSuccessfullyCalled_HasExpectedNodesAndEdges()
        {
            var uniedges = new List<(string, string, int)> {
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
            var nodeNames = new List<string> { "A", "B", "C", "D", "E", "F", "G", "H" };

            var nodes = FakeGraphFactory.FakeNodes(nodeNames);
            var edges = FakeGraphFactory.FakeRoute(uniedges, nodes);
            var aliases = new List<FacetTable>();

            Mock<RepositoryRegistry> registry = FakeRegistryFactory.MockRepositoryRegistry(nodes, edges, aliases);

            var finder = new PathFinder(edges);

            Assert.NotNull(finder);

        }

        [Fact]
        public void Find_WhenStartHasPathToStop_ShouldBeShortestPath()
        {
            // Arrange
            var finder = new PathFinder(FakeGraphFactory.CreateSimpleGraph());

            const string start_table = "A";
            List<string> destination_tables = new List<string>() { "H" };

            // Act
            var result = finder.Find(start_table, destination_tables);

            // Assert
            Assert.Single(result);
            Assert.Equal(3, result[0].Count);
            Assert.Equal(Tuple.Create("A", "B"), result[0][0].Key);
            Assert.Equal(Tuple.Create("B", "F"), result[0][1].Key);
            Assert.Equal(Tuple.Create("F", "H"), result[0][2].Key);
        }

        [Fact]
        public void Find_WhenStartAndStopAreSwitched_ShouldBeReversedPath()
        {
            // Arrange
            var finder = new PathFinder(FakeGraphFactory.CreateSimpleGraph());

            const string startTable = "H";
            const string destinationTable = "A";

            // Act
            var route = finder.Find(startTable, destinationTable);
            var routeReversed = finder.Find(destinationTable, startTable);

            // Assert
            string trail = String.Join('-', route.ToTrail());
            Assert.Equal("H-F-B-A", trail);

            string trailReversed = String.Join('-', routeReversed.ToTrail());
            Assert.Equal("A-B-F-H", trailReversed);
        }

        [Fact]
        public void ToCSV_AnyState_ShouldBeStringOfDelimitedLines()
        {
            // Arrange
            var finder = new PathFinder(FakeGraphFactory.CreateSimpleGraph());

            // Act
            var result = finder.Graph.ToCSV();

            // Assert
            const string expected = "A;B;7\nA;C;8\nB;A;7\nB;F;2\nC;A;8\nC;F;6\nC;G;4\nD;F;8\nE;H;1\nF;B;2\nF;C;6\nF;D;8\nF;G;9\nF;H;3\nG;C;4\nG;F;9\nH;E;1\nH;F;3\n";
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Find_WhenStartAndStopsAreNeighbours_IsSingleStep()
        {
            using (var container = CreateDependencyContainer())
            using (var scope = container.BeginLifetimeScope())
            {
                // Arrange
                var graph = scope.Resolve<IPathFinder>();
                // Act
                Route route = graph.Find("tbl_locations", "tbl_site_locations");
                // Assert
                Assert.NotNull(route);
                Assert.Single(route);
                Assert.Equal("tbl_locations", route[0].SourceName);
                Assert.Equal("tbl_site_locations", route[0].TargetName);
            }
        }

        [Fact]
        public void Find_WhenStartEqualsStop_ReturnsEmptyRoute()
        {
            var graph = CreateFacetsGraphByFakeContext(FacetContext);

            Route route = graph.Find("tbl_locations", "tbl_locations");

            Assert.NotNull(route);
            Assert.Empty(route);
        }

    }
}
