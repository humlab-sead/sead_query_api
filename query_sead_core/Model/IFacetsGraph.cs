using System;
using System.Collections.Generic;

namespace QueryFacetDomain {
    public interface IFacetsGraph {
        IEnumerable<FacetDefinition> AliasFacets { get; }
        Dictionary<Tuple<string, string>, GraphEdge> Edges { get; }
        Dictionary<int, GraphNode> NodeIds { get; }
        Dictionary<string, GraphNode> Nodes { get; }
        Dictionary<int, Dictionary<int, int>> Weights { get; }
        GraphEdge GetEdge(int sourceId, int targetId);
        GraphEdge GetEdge(string source, string target);
        string ResolveName(string tableName);
    }
}