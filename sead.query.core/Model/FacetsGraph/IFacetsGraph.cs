using System;
using System.Collections.Generic;

namespace SeadQueryCore
{
    using NodesDictS = Dictionary<string, GraphNode>;
    using NodesDictI = Dictionary<int, GraphNode>;

    public interface IFacetsGraph {
        //List<Facet> AliasFacets { get; }
        Dictionary<Tuple<string, string>, GraphEdge> Edges { get; }
        NodesDictI NodesIds { get; }
        NodesDictS Nodes { get; }
        Dictionary<int, Dictionary<int, int>> Weights { get; }
        GraphEdge GetEdge(int sourceId, int targetId);
        GraphEdge GetEdge(string source, string target);
        GraphRoute Find(string start_table, string destination_table);
        List<GraphRoute> Find(string start_table, List<string> destination_tables);
        bool IsAlias(string tableName);
        string ResolveTargetName(string aliasOrTable);
        string ToCSV();
        string ResolveAliasName(string targetTableName);
    }
}