using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace SeadQueryCore;

using WeightDictionary = Dictionary<int, Dictionary<int, int>>;

public class FacetsGraph : IFacetsGraph
{
    public IEnumerable<Table> Tables { get; private set; }
    public IEnumerable<TableRelation> Relations { get; private set; }

    public TableLookup TableLookup { get; private set; }
    public RelationLookup RelationLookup { get; private set; }
    public IEnumerable<FacetTable> AliasedFacetTables { get; private set; }

    public FacetsGraph(IEnumerable<Table> tables, IEnumerable<TableRelation> relations, IEnumerable<FacetTable> aliases, bool bidirectional = true)
    {
        Tables = tables;
        Relations = relations;

        TableLookup = new TableLookup(tables);
        RelationLookup = new RelationLookup(relations, bidirectional);
        AliasedFacetTables = aliases;
    }

    public WeightDictionary ToWeightGraph() => Relations
        .GroupBy(p => p.SourceId, (key, g) => (SourceId: key, TargetWeights: g.ToDictionary(x => x.TargetId, x => x.Weight)))
        .ToDictionary(x => x.SourceId, y => y.TargetWeights);

    public List<GraphRoute> Find(string start_table, List<string> destination_tables, bool reduce = true)
    {
        // Make sure that start table doesn't exists in list of destination tables...
        destination_tables = destination_tables.Where(z => z != start_table).ToList();

        var routes = destination_tables.Select(z => Find(start_table, z)).ToList();

        if (reduce)
        {
            return GraphUtility.Reduce(routes);
        }

        return routes;
    }

    public GraphRoute Find(string startTable, string destinationTable)
    {
        return Find(TableLookup[startTable].TableId, TableLookup[destinationTable].TableId);
    }

    public GraphRoute Find(int startId, int destId)
    {
        IEnumerable<int> trail = new DijkstrasGraph<int>(ToWeightGraph())
            .shortest_path(startId, destId);

        if (trail == null)
            throw new ArgumentOutOfRangeException($"No route found between {startId} and {destId}");

        return ToGraphRoute(trail.Concat([startId]));
    }

    private GraphRoute ToGraphRoute(IEnumerable<int> trail)
        => new GraphRoute(ToEdges(trail.Reverse()));

    private IEnumerable<TableRelation> ToEdges(IEnumerable<int> trail)
        => trail
            .Select(x => TableLookup[x])
            .PairWise((a, b) => RelationLookup.GetEdge(a.TableOrUdfName, b.TableOrUdfName));

    public string ToCSV()
    {
        StringBuilder sb = new StringBuilder();
        foreach (TableRelation edge in RelationLookup)
            sb.Append($"{edge.SourceName};{edge.TargetName};{edge.Weight}\n");
        return sb.ToString();
    }

    public FacetTable GetAliasedFacetTable(string aliasName)
        => AliasedFacetTables.Where(x => x.Alias == aliasName).FirstOrDefault();
}
