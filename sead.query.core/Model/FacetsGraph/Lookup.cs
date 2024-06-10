
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
    public RelationLookup(IEnumerable<TableRelation> relation, bool bidirectional = true)
    {
        if (bidirectional)
        {
            relation = relation.Concat(GraphUtility.Reverse(relation));
        }
        Edges = relation;
        KeyLookup = relation.ToDictionary(z => z.Key);
        IdKeyLookup = relation.ToDictionary(z => z.IdKey);
    }

    public TableRelation GetEdge(string source, string target)
        => KeyLookup[Tuple.Create(source, target)];

    public TableRelation GetEdge(int sourceId, int targetId)
        => IdKeyLookup[Tuple.Create(sourceId, targetId)];

    IEnumerator IEnumerable.GetEnumerator()
    {
        return Edges.GetEnumerator();
    }

    public WeightDictionary ToWeightGraph() => Edges
        .GroupBy(p => p.SourceId, (key, g) => (SourceId: key, TargetWeights: g.ToDictionary(x => x.TargetId, x => x.Weight)))
        .ToDictionary(x => x.SourceId, y => y.TargetWeights);
}
