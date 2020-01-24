using System;
using System.Collections.Generic;

namespace SeadQueryCore
{
    using NodesDictS = Dictionary<string, Table>;
    using NodesDictI = Dictionary<int, Table>;

    public interface IFacetsGraph {
        Dictionary<Tuple<string, string>, TableRelation> Edges { get; }
        NodesDictI NodesIds { get; }
        NodesDictS Nodes { get; }
        Dictionary<int, Dictionary<int, int>> Weights { get; }
        TableRelation GetEdge(int sourceId, int targetId);
        TableRelation GetEdge(string source, string target);
        GraphRoute Find(string start_table, string destination_table);
        List<GraphRoute> Find(string start_table, List<string> destination_tables, bool reduce=true);
        Dictionary<string, FacetTable> AliasTables { get; }
        string ToCSV();
    }
}