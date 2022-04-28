using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SeadQueryCore;

namespace SQT.Mocks
{
    public static class FakeFacetGraphFactory
    {
        public class FakeGraphGenerator
        {
            public List<Table> Nodes { get; }
            public Dictionary<string, Table> NodeMap { get; }
            public List<TableRelation> Edges { get; } = new List<TableRelation>();

            public FakeGraphGenerator(int n)
            {
                Nodes = GenerateNodes(n).ToList();
                NodeMap = Nodes.ToDictionary(z => z.TableOrUdfName);
            }

            public IEnumerable<Table> GenerateNodes(int n)
            {
                for (var i = 1; i <= n; i++)
                    yield return new Table() { TableId = i, TableOrUdfName = Convert.ToChar('A' + i - 1).ToString() };
            }

            public FakeGraphGenerator Add(string x, string y, int w)
            {
                var edge = new TableRelation()
                {
                    TableRelationId = Edges.Count + 1,
                    SourceTable = NodeMap[x],
                    TargetTable = NodeMap[y],
                    Weight = w,
                    SourceColumName = $"{x.ToLower()}{y.ToLower()}_key",
                    TargetColumnName = $"{x.ToLower()}{y.ToLower()}_key"
                };
                Edges.Add(edge);
                return this;
            }

            public FakeGraphGenerator Add(List<(string x, string y, int w)> xyws)
            {
                foreach (var (x, y, w) in xyws)
                {
                    Add(x, y, w);
                }
                return this;
            }

            public FakeGraphGenerator Add(string x, Dictionary<string, int> yw)
            {
                foreach (var y in yw.Keys)
                {
                    Add(x, y, yw[y]);
                }
                return this;
            }
        }

        public static IFacetsGraph CreateSimpleGraph()
        {
            var generator = new FakeGraphGenerator(8);
            generator.Add("A", new Dictionary<string, int> { { "B", 7 }, { "C", 8 } });
            generator.Add("B", new Dictionary<string, int> { { "A", 7 }, { "F", 2 } });
            generator.Add("C", new Dictionary<string, int> { { "A", 8 }, { "F", 6 }, { "G", 4 } });
            generator.Add("D", new Dictionary<string, int> { { "F", 8 } });
            generator.Add("E", new Dictionary<string, int> { { "H", 1 } });
            generator.Add("F", new Dictionary<string, int> { { "B", 2 }, { "C", 6 }, { "D", 8 }, { "G", 9 }, { "H", 3 } });
            generator.Add("G", new Dictionary<string, int> { { "C", 4 }, { "F", 9 } });
            generator.Add("H", new Dictionary<string, int> { { "E", 1 }, { "F", 3 } });
            var facetsGraph = new FacetsGraph(
                nodes: generator.Nodes,
                edges: generator.Edges,
                aliases: new List<FacetTable>()
            );
            return facetsGraph;
        }
    }
}
