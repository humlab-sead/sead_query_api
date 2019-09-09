using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace sead.query.test.data.Entities
{
    public partial class GraphEdge
    {
        public int RelationId { get; set; }
        public int SourceTableId { get; set; }
        public int TargetTableId { get; set; }
        public int Weight { get; set; }
        public string SourceColumnName { get; set; }
        public string TargetColumnName { get; set; }

        [JsonIgnore]
        public virtual GraphNode SourceTable { get; set; }
        [JsonIgnore]
        public virtual GraphNode TargetTable { get; set; }
    }
}
