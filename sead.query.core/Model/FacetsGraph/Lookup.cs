
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using System;

namespace SeadQueryCore;

using EdgeKey = Tuple<string, string>;
using EdgeIdKey = Tuple<int, int>;
using WeightDictionary = Dictionary<int, Dictionary<int, int>>;

public class TableLookup(IEnumerable<Table> tables)
{
    public Dictionary<string, Table> Name2Node { get; private set; } = tables.ToDictionary(x => x.Name);
    public Dictionary<int, Table> Id2Node { get; private set; } = tables.ToDictionary(x => x.Id);

    public Table this[int index]
    {
        get
        {
            return Id2Node[index];
        }
    }
    public Table this[string name]
    {
        get
        {
            return Name2Node[name];
        }
    }
}

public class RelationLookup : IEnumerable
{
    public Dictionary<EdgeKey, TableRelation> KeyLookup { get; private set; }
    public Dictionary<EdgeIdKey, TableRelation> IdKeyLookup { get; private set; }
    public IEnumerable<TableRelation> Edges { get; private set; }
    public RelationLookup(IEnumerable<TableRelation> edges, bool bidirectional = true)
    {
        if (bidirectional)
        {
            edges = edges.Concat(ReversedEdges(edges));
        }
        Edges = edges;
        KeyLookup = edges.ToDictionary(z => z.Key);
        IdKeyLookup = edges.ToDictionary(z => z.IdKey);
    }

    public TableRelation GetEdge(string source, string target)
        => KeyLookup[Tuple.Create(source, target)];

    public TableRelation GetEdge(int sourceId, int targetId)
        => IdKeyLookup[Tuple.Create(sourceId, targetId)];

    IEnumerator IEnumerable.GetEnumerator()
    {
        return Edges.GetEnumerator();
    }
    public IEnumerable<TableRelation> ReversedEdges(IEnumerable<TableRelation> edges)
    {
        return edges
            .Where(z => z.SourceId != z.TargetId)
            .Select(x => (TableRelation)x.Reverse())
            .Where(z => !edges.Any(w => w.Equals(z)));
    }

    public WeightDictionary ToWeightGraph() => Edges
        .GroupBy(p => p.SourceId, (key, g) => (SourceId: key, TargetWeights: g.ToDictionary(x => x.TargetId, x => x.Weight)))
        .ToDictionary(x => x.SourceId, y => y.TargetWeights);
}
