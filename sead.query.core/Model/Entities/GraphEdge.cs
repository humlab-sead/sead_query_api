using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SeadQueryCore
{

    public class GraphEdge {
        public int EdgeId { get; set; }
        public int SourceNodeId { get; set; }
        public int TargetNodeId { get; set; }
        public int Weight { get; set; }

        public string SourceKeyName { get; set; }
        public string TargetKeyName { get; set; }

        [JsonIgnore] private GraphNode _SourceNode, _TargetNode;

        public GraphNode SourceNode { get { return _SourceNode; } set { _SourceNode = value; SourceNodeId = value?.NodeId ?? SourceNodeId;  } }
        public GraphNode TargetNode { get { return _TargetNode; } set { _TargetNode = value; TargetNodeId = value?.NodeId ?? TargetNodeId; } }

        [JsonIgnore] public string SourceName { get { return SourceNode.TableName; } }
        [JsonIgnore] public string TargetName { get { return TargetNode.TableName; } }

        public GraphEdge Clone()
        {
            return new GraphEdge() {
                EdgeId = EdgeId + 1000,
                Weight = Weight,
                SourceNodeId = SourceNodeId,
                TargetNodeId = TargetNodeId,
                SourceNode = SourceNode,
                TargetNode = TargetNode,
                SourceKeyName = SourceKeyName,
                TargetKeyName = TargetKeyName
            };
        }

        public GraphEdge Reverse()
        {
            var x = Clone();
            x.EdgeId = -x.EdgeId;
            (x.SourceNodeId, x.TargetNodeId) = (x.TargetNodeId, x.SourceNodeId);
            (x.SourceNode, x.TargetNode) = (x.TargetNode, x.SourceNode);
            (x.SourceKeyName, x.TargetKeyName) = (x.TargetKeyName, x.SourceKeyName);
            return x;
        }

        public GraphEdge Alias(GraphNode node, GraphNode alias)
        {
            var x = Clone();
            if (node.NodeId == SourceNode.NodeId)
                x.SourceNode = alias;
            else
                x.TargetNode = alias;
            return x;
        }

        public Tuple<string, string> Key { get { return new Tuple<string, string>(SourceName, TargetName); } }

        public bool EqualAs(GraphEdge x)
        {
            //return (SourceNodeId == x.SourceNodeId) && (TargetNodeId == x.TargetNodeId);
            return (SourceName == x.SourceName) && (TargetName == x.TargetName);
        }

        public string ToStringPair()
        {
            return $"{SourceName}/{TargetName}";
        }
    }
}
