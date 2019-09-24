using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace SeadQueryCore
{
    using NodesDictS = Dictionary<string, GraphNode>;
    using NodesDictI = Dictionary<int, GraphNode>;
    using WeightDictionary = Dictionary<int, Dictionary<int, int>>;

    public interface IFacetGraphFactory
    {
        IFacetsGraph Build(List<GraphNode> nodes, List<GraphEdge> edges, List<Facet> aliasFacets);

    }
}