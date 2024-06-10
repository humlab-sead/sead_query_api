using Moq;
using SeadQueryCore;
using System;
using System.Collections.Generic;
using Xunit;
using Autofac;
using System.Linq;
using SQT.Infrastructure;
using SeadQueryInfra;
using SQT.Mocks;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.Sqlite;
using System.Threading.Tasks;
using System.IO;

namespace SQT.Model
{
    [Collection("SeadJsonFacetContextFixture")]
    public class FacetsGraphTests : DisposableFacetContextContainer
    {
        public FacetsGraphTests(SeadJsonFacetContextFixture fixture) : base(fixture)
        {
        }

        private IFacetsGraph CreateFacetsGraphByFakeContext(IFacetContext testContext)
        {
            return ScaffoldUtility.DefaultFacetsGraph(testContext);
        }

        private IContainer CreateDependencyContainer()
        {
            var folder = Path.Combine(ScaffoldUtility.GetRootFolder(), "Infrastructure", "Data", "Json");
            var container = DependencyService.CreateContainer(FacetContext, folder, null);
            return container;
        }

        [Fact]
        public void GetEdge_ByNodeNames_WhenEdgeExists_ReturnsEdge()
        {
            // Arrange
            var facetsGraph = FakeFacetGraphFactory.CreateSimpleGraph();

            const string source = "D";
            const string target = "F";

            // Act
            var result = facetsGraph.RelationLookup.GetEdge(source, target);

            // Assert
            Assert.Equal(source, result.SourceName);
            Assert.Equal(target, result.TargetName);
            Assert.Equal(8, result.Weight);
        }

        [Fact]
        public void GetEdge_ByNodeId_WhenEdgeExists_ReturnsEdge()
        {
            // Arrange
            var facetsGraph = FakeFacetGraphFactory.CreateSimpleGraph();

            const int sourceId = 4;
            const int targetId = 6;

            // Act
            var result = facetsGraph.RelationLookup.GetEdge(sourceId, targetId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(4, result.SourceTableId);
            Assert.Equal(6, result.TargetTableId);
            Assert.Equal(8, result.Weight);
        }

        //[Fact]
        //public void IsAlias_WhenGraphHasAlias_ShouldBeTrue()
        //{
        //    // Arrange
        //    var facetsGraph = MockFacetGraphGenerator.CreateSimpleGraph();

        //    const string name = "X";

        //    // Act
        //    var result = facetsGraph.IsAlias(name);

        //    // Assert
        //    Assert.True(result);
        //}

        //[Fact]
        //public void ResolveTargetName_WhenGraphHasAlias_ShouldBeTargetName()
        //{
        //    // Arrange
        //    var facetsGraph = MockFacetGraphGenerator.CreateSimpleGraph();

        //    const string aliasOrTable = "X";

        //    // Act
        //    var result = facetsGraph.ResolveTargetName(
        //        aliasOrTable);

        //    // Assert
        //    Assert.Equal("A", result);
        //}

        //[Fact]
        //public void ResolveAliasName_WhenGraphHasAlias_ShouldBeAliasName()
        //{
        //    // Arrange
        //    var facetsGraph = MockFacetGraphGenerator.CreateSimpleGraph();

        //    const string aliasOrTable = "X";

        //    // Act
        //    var result = facetsGraph.ResolveAliasName(aliasOrTable);

        //    // Assert
        //    Assert.Equal("X", result);
        //}

        [Fact]
        public void Find_WhenStartHasPathToStop_ShouldBeShortestPath()
        {
            // Arrange
            var facetsGraph = FakeFacetGraphFactory.CreateSimpleGraph();

            const string start_table = "A";
            List<string> destination_tables = new List<string>() { "H" };

            // Act
            var result = facetsGraph.Find(start_table, destination_tables);

            // Assert
            Assert.Single(result);
            Assert.Equal(3, result[0].Items.Count);
            Assert.Equal(Tuple.Create("A", "B"), result[0].Items[0].Key);
            Assert.Equal(Tuple.Create("B", "F"), result[0].Items[1].Key);
            Assert.Equal(Tuple.Create("F", "H"), result[0].Items[2].Key);
        }

        [Fact]
        public void Find_WhenStartAndStopAreSwitched_ShouldBeReversedPath()
        {
            // Arrange
            var facetsGraph = FakeFacetGraphFactory.CreateSimpleGraph();

            const string startTable = "H";
            const string destinationTable = "A";

            // Act
            var route = facetsGraph.Find(startTable, destinationTable);
            var routeReversed = facetsGraph.Find(destinationTable, startTable);

            // Assert
            string trail = String.Join('-', route.Trail());
            Assert.Equal("H-F-B-A", trail);

            string traiiReversed = String.Join('-', routeReversed.Trail());
            Assert.Equal("A-B-F-H", traiiReversed);
        }

        [Fact]
        public void ToCSV_AnyState_ShouldBeStringOfDelimitedLines()
        {
            // Arrange
            var facetsGraph = FakeFacetGraphFactory.CreateSimpleGraph();

            // Act
            var result = facetsGraph.ToCSV();

            // Assert
            const string expected = "A;B;7\nA;C;8\nB;A;7\nB;F;2\nC;A;8\nC;F;6\nC;G;4\nD;F;8\nE;H;1\nF;B;2\nF;C;6\nF;D;8\nF;G;9\nF;H;3\nG;C;4\nG;F;9\nH;E;1\nH;F;3\n";
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Build_WhenResolvedByIoC_HasExpectedEdges()
        {
            using (var container = CreateDependencyContainer())
            using (var scope = container.BeginLifetimeScope())
            {
                var service = scope.Resolve<IFacetsGraph>();
                Assert.True(service.RelationLookup.Edges.Any());
            }
        }

        [Fact]
        public void Find_WhenStartAndStopsAreNeighbours_IsSingleStep()
        {
            using (var container = CreateDependencyContainer())
            using (var scope = container.BeginLifetimeScope())
            {
                // Arrange
                var graph = scope.Resolve<IFacetsGraph>();
                // Act
                GraphRoute route = graph.Find("tbl_locations", "tbl_site_locations");
                // Assert
                Assert.NotNull(route);
                Assert.Single(route.Items);
                Assert.Equal("tbl_locations", route.Items[0].SourceName);
                Assert.Equal("tbl_site_locations", route.Items[0].TargetName);
            }
        }

        [Fact]
        public void Find_WhenStartEqualsStop_ReturnsEmptyRoute()
        {
            var graph = CreateFacetsGraphByFakeContext(FacetContext);

            GraphRoute route = graph.Find("tbl_locations", "tbl_locations");

            Assert.NotNull(route);
            Assert.Empty(route.Items);
        }
        //public static DbContextOptions Initialize(DbConnection connection)
        //{
        //    var seeder = new FakeFacetContextJsonSeeder();
        //    var options = SqliteInMemoryContextOptionsFactory.Create(connection);
        //    using (var context = new FacetContext(options)) {
        //        context.Database.EnsureCreated();
        //    }
        //    using (var context = new FacetContext(options)) {
        //        seeder.Seed(context);
        //    }
        //    return options;
        //}

        [Fact]
        public async Task TestMethod_UsingSqliteInMemoryProvider_Success()
        {
            var folder = ScaffoldUtility.JsonDataFolder();

            using (var connection = new SqliteConnection("DataSource=:memory:;Foreign Keys = False"))
            {
                connection.Open();

                var options = new DbContextOptionsBuilder<FacetContext>()
                    .UseSqlite(connection) // Set the connection explicitly, so it won't be closed automatically by EF
                    .Options;

                using (var context = JsonSeededFacetContextFactory.Create(options, Fixture))
                {
                    var count = await context.FacetGroups.CountAsync();
                    Assert.True(count > 0);
                    var u = await context.FacetGroups.FirstOrDefaultAsync(group => group.FacetGroupKey == "DOMAIN");
                    Assert.NotNull(u);
                    Assert.Equal(999, u.FacetGroupId);
                }
            }
        }
    }
}
