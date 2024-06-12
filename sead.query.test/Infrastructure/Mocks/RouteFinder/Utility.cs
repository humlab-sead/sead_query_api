using System;
using System.Collections.Generic;
using System.Linq;
using SeadQueryCore;

namespace SQT.Mocks;

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
