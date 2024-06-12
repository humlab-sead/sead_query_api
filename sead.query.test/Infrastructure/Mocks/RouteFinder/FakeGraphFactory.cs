using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using SeadQueryCore;
using SeadQueryInfra;

namespace SQT.Mocks;

public static class FakeGraphFactory
{

    public static List<(char, char, int)> EdgesAsValueTuples()
    {
        return [
            ('A', 'B', 7),
            ('A', 'C', 8),
            ('B', 'A', 7),
            ('B', 'F', 2),
            ('C', 'A', 8),
            ('C', 'F', 6),
            ('C', 'G', 4),
            ('D', 'F', 8),
            ('E', 'H', 1),
            ('F', 'B', 2),
            ('F', 'C', 6),
            ('F', 'D', 8),
            ('F', 'G', 9),
            ('F', 'H', 3),
            ('G', 'C', 4),
            ('G', 'F', 9),
            ('H', 'E', 1),
            ('H', 'F', 3)
        ];
    }

    public static Dictionary<char, Dictionary<char, int>> EdgesAsDictionary()
    {
        return new Dictionary<char, Dictionary<char, int>> {
            { 'A', new Dictionary<char, int>() { { 'B', 7 }, { 'C', 8 } } },
            { 'B', new Dictionary<char, int>() { { 'A', 7 }, { 'F', 2 } } },
            { 'C', new Dictionary<char, int>() { { 'A', 8 }, { 'F', 6 }, { 'G', 4 } } },
            { 'D', new Dictionary<char, int>() { { 'F', 8 } } },
            { 'E', new Dictionary<char, int>() { { 'H', 1 } } },
            { 'F', new Dictionary<char, int>() { { 'B', 2 }, { 'C', 6 }, { 'D', 8 }, { 'G', 9 }, { 'H', 3 } } },
            { 'G', new Dictionary<char, int>() { { 'C', 4 }, { 'F', 9 } } },
            { 'H', new Dictionary<char, int>() { { 'E', 1 }, { 'F', 3 } } }
        };
    }
    
    public static List<TableRelation> CreateSimpleGraph()
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
        return generator.Edges;
    }

    public static List<Table> FakeNodes(List<string> nodeNames) => nodeNames.Select((x, i) => new Table
    {
        TableId = i,
        TableOrUdfName = x,
        PrimaryKeyName = x.ToLower()
    }).ToList();

    public static List<TableRelation> FakeRoute(List<(string, string, int)> uniedges, List<Table> nodes) => uniedges.Select((x, i) => new TableRelation
    {
        TableRelationId = i,
        Weight = x.Item3,
        SourceTable = nodes.Where(n => n.TableOrUdfName == x.Item1).First(),
        TargetTable = nodes.Where(n => n.TableOrUdfName == x.Item2).First(),
        SourceColumName = x.Item1.ToLower(),
        TargetColumnName = x.Item1.ToLower()
    }).ToList();


    public static RouteFinder FakeSimpleRouteFinder()
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

        var nodes = FakeNodes(nodeNames);
        var edges = FakeRoute(uniedges, nodes);
        var aliases = new List<FacetTable>();

        Mock<RepositoryRegistry> registry = FakeRegistryFactory.MockRepositoryRegistry(nodes, edges, aliases);

        var finder = new RouteFinder(registry.Object, edges);

        return finder;

    }
}
