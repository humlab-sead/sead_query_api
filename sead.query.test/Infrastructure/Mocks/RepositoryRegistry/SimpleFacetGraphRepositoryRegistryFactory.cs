using Moq;
using SeadQueryCore;
using System.Collections.Generic;
using System.Linq;

namespace SQT.Mocks
{
    internal static class SimpleFacetGraphRepositoryRegistryFactory
    {
        public static List<Table> Nodes = new List<Table>() {
            new Table { TableId = 1, TableOrUdfName = "A" },
            new Table { TableId = 2, TableOrUdfName = "B" },
            new Table { TableId = 3, TableOrUdfName = "C" },
            new Table { TableId = 4, TableOrUdfName = "D" },
            new Table { TableId = 5, TableOrUdfName = "E" },
            new Table { TableId = 6, TableOrUdfName = "F" },
            new Table { TableId = 7, TableOrUdfName = "G" },
            new Table { TableId = 8, TableOrUdfName = "H" }
        };

        public static Dictionary<string, Table> NodesDict = Nodes.ToDictionary(z => z.TableOrUdfName);

        public static List<TableRelation> Edges = new List<TableRelation>() {
            new TableRelation() { TableRelationId = 0, SourceTable = NodesDict["A"], TargetTable = NodesDict["B"], Weight = 7 },
            new TableRelation() { TableRelationId = 0, SourceTable = NodesDict["A"], TargetTable = NodesDict["C"], Weight = 8 },
            new TableRelation() { TableRelationId = 0, SourceTable = NodesDict["B"], TargetTable = NodesDict["F"], Weight = 2 },
            new TableRelation() { TableRelationId = 0, SourceTable = NodesDict["C"], TargetTable = NodesDict["F"], Weight = 6 },
            new TableRelation() { TableRelationId = 0, SourceTable = NodesDict["C"], TargetTable = NodesDict["G"], Weight = 4 },
            new TableRelation() { TableRelationId = 0, SourceTable = NodesDict["D"], TargetTable = NodesDict["F"], Weight = 8 },
            new TableRelation() { TableRelationId = 0, SourceTable = NodesDict["E"], TargetTable = NodesDict["H"], Weight = 1 },
            new TableRelation() { TableRelationId = 0, SourceTable = NodesDict["F"], TargetTable = NodesDict["G"], Weight = 9 },
            new TableRelation() { TableRelationId = 0, SourceTable = NodesDict["F"], TargetTable = NodesDict["H"], Weight = 3 }
        };

        public static IRepositoryRegistry CreateMockRegistry()
        {
            var mockRegistry = new Mock<IRepositoryRegistry>();

            mockRegistry.Setup(x => x.Tables.GetAll())
                .Returns(Nodes);

            mockRegistry.Setup(x => x.Relations.GetAll())
                .Returns(Edges);

            mockRegistry.Setup(x => x.FacetTables.FindThoseWithAlias())
                .Returns(new List<FacetTable>());

            return mockRegistry.Object;
        }
    }
}
