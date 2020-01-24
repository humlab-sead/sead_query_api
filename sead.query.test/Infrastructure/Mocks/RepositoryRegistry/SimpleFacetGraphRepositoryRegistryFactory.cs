using Moq;
using SeadQueryCore;
using System.Collections.Generic;
using System.Linq;

namespace SeadQueryTest.Mocks
{
    internal static class SimpleFacetGraphRepositoryRegistryFactory
    {
        public static IRepositoryRegistry CreateMockRegistry()
        {
            var nodes = new List<Table>() {
                new Table { TableId = 0, TableOrUdfName = "A" },
                new Table { TableId = 0, TableOrUdfName = "B" },
                new Table { TableId = 0, TableOrUdfName = "C" },
                new Table { TableId = 0, TableOrUdfName = "D" },
                new Table { TableId = 0, TableOrUdfName = "E" },
                new Table { TableId = 0, TableOrUdfName = "F" },
                new Table { TableId = 0, TableOrUdfName = "G" },
                new Table { TableId = 0, TableOrUdfName = "H" }
            };

            var nodesDict = nodes.ToDictionary(z => z.TableOrUdfName);

            var edges = new List<TableRelation>() {
                new TableRelation() { TableRelationId = 0, SourceTable = nodesDict["A"], TargetTable = nodesDict["B"], Weight = 7 },
                new TableRelation() { TableRelationId = 0, SourceTable = nodesDict["A"], TargetTable = nodesDict["C"], Weight = 8 },
                new TableRelation() { TableRelationId = 0, SourceTable = nodesDict["B"], TargetTable = nodesDict["F"], Weight = 2 },
                new TableRelation() { TableRelationId = 0, SourceTable = nodesDict["C"], TargetTable = nodesDict["F"], Weight = 6 },
                new TableRelation() { TableRelationId = 0, SourceTable = nodesDict["C"], TargetTable = nodesDict["G"], Weight = 4 },
                new TableRelation() { TableRelationId = 0, SourceTable = nodesDict["D"], TargetTable = nodesDict["F"], Weight = 8 },
                new TableRelation() { TableRelationId = 0, SourceTable = nodesDict["E"], TargetTable = nodesDict["H"], Weight = 1 },
                new TableRelation() { TableRelationId = 0, SourceTable = nodesDict["F"], TargetTable = nodesDict["G"], Weight = 9 },
                new TableRelation() { TableRelationId = 0, SourceTable = nodesDict["F"], TargetTable = nodesDict["H"], Weight = 3 }
            };

            var mockRegistry = new Mock<IRepositoryRegistry>();

            mockRegistry.Setup(x => x.Tables.GetAll())
                .Returns(nodes);

            mockRegistry.Setup(x => x.TableRelations.GetAll())
                .Returns(edges);

            mockRegistry.Setup(x => x.FacetTables.AliasTablesDict())
                .Returns(new Dictionary<string, FacetTable>());

            return mockRegistry.Object;
        }
    }


}
