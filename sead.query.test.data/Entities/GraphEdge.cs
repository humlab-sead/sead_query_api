using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace sead.query.test.data.Entities
{
    public partial class GraphEdge
    {
        public int EdgeId { get; set; }
        public int SourceNodeId { get; set; }
        public int TargetNodeId { get; set; }
        public int Weight { get; set; }
        public string SourceKeyName { get; set; }
        public string TargetKeyName { get; set; }

        [JsonIgnore]
        public virtual GraphNode SourceNode { get; set; }
        [JsonIgnore]
        public virtual GraphNode TargetNode { get; set; }
    }
}
