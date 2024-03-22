using Moq;
using SeadQueryCore;
using SeadQueryInfra;
using SQT.Mocks;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using SQT.Infrastructure;

namespace SQT.Model
{
    [Collection("SeadJsonFacetContextFixture")]
    public class FacetGraphFactoryTests : DisposableFacetContextContainer
    {
        public FacetGraphFactoryTests(SeadJsonFacetContextFixture fixture) : base(fixture)
        {
        }

        [Fact]
        public void Build_WhenSuccessfullyCalled_HasExpectedNodesAndEdges()
        {
            //var bidirectedges = new List<(string, string, int)> {
            //    ("A", "B", 7 ), ( "A", "C", 8 ),
            //    ("B", "A", 7 ), ( "B", "F", 2 ),
            //    ("C", "A", 8 ), ( "C", "F", 6 ), ( "C", "G", 4 ),
            //    ("D", "F", 8 ),
            //    ("E", "H", 1 ),
            //    ("F", "B", 2 ), ( "F", "C", 6 ), ( "F", "D", 8 ), ( "F", "G", 9 ), ( "F", "H", 3),
            //    ("G", "C", 4 ), ( "G", "F", 9 ),
            //    ("H", "E", 1 ), ( "H", "F", 3 )
            //};
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

            var nodes = CreateTables(nodeNames);
            var edges = CreateTableRelations(uniedges, nodes);
            var aliases = new List<FacetTable>();

            Mock<RepositoryRegistry> mockRepository = MockRepositoryRegistry(nodes, edges, aliases);

            var factory = new FacetGraphFactory(mockRepository.Object);

            // Act
            var result = factory.Build();

            // Assert
            Assert.Equal(nodes.Count, result.NodeContainer.Nodes.Count());
            Assert.Equal(2 * uniedges.Count, result.EdgeContaniner.Edges.Count());
        }

        private static Mock<RepositoryRegistry> MockRepositoryRegistry(List<Table> nodes, IEnumerable<TableRelation> edges, List<FacetTable> aliases)
        {
            var mockRepository = new Mock<RepositoryRegistry>(It.IsAny<FacetConfig2>());

            mockRepository.Setup(r => r.Tables.GetAll()).Returns(nodes);
            mockRepository.Setup(r => r.TableRelations.GetAll()).Returns(edges);
            mockRepository.Setup(r => r.FacetTables.FindThoseWithAlias()).Returns(aliases);

            return mockRepository;
        }

        private static List<Table> CreateTables(List<string> nodeNames) => nodeNames.Select((x, i) => new Table
        {
            TableId = i,
            TableOrUdfName = x,
            PrimaryKeyName = x.ToLower()
        }).ToList();

        private static IEnumerable<TableRelation> CreateTableRelations(List<(string, string, int)> uniedges, List<Table> nodes) => uniedges.Select((x, i) => new TableRelation
        {
            TableRelationId = i,
            Weight = x.Item3,
            SourceTable = nodes.Where(n => n.TableOrUdfName == x.Item1).First(),
            TargetTable = nodes.Where(n => n.TableOrUdfName == x.Item2).First(),
            SourceColumName = x.Item1.ToLower(),
            TargetColumnName = x.Item1.ToLower()
        });
    }
}
