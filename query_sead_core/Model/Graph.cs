using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace QuerySeadDomain
{

    public class GraphNode {

        public int TableId { get; set; }
        public string TableName { get; set; }

        public GraphNode makeAlias(string alias)
        {
            return new GraphNode() {
                TableId = 1000 + this.TableId,
                TableName = alias
            };
        }
    }

    public class GraphEdge {

        public int RelationId { get; set; }
        public int SourceTableId { get; set; }
        public int TargetTableId { get; set; }
        public int Weight { get; set; }

        public string SourceColumnName { get; set; }
        public string TargetColumnName { get; set; }

        //public string ExtraCritera { get; set; } = ""; // Not used for SEAD

        public GraphNode SourceTable { get; set; }
        public GraphNode TargetTable { get; set; }

        // Computed properties
        [JsonIgnore]
        public string SourceTableName { get { return SourceTable.TableName; } }
        [JsonIgnore]
        public string TargetTableName { get { return TargetTable.TableName; } }

        [JsonIgnore]
        public string SourceName { get { return $"{SourceTableName}.\"{SourceColumnName}\""; } }
        [JsonIgnore]
        public string TargetName { get { return $"{TargetTableName}.\"{TargetColumnName}\""; } }

        // Note! Only simple, non-composite key allows (just id-2-id), and for now no extra critera
        public string GetSqlJoinClause(string joinType)
            => $" ({SourceName} = {TargetName})\n";

        public GraphEdge Clone()
        {
            return new GraphEdge() {
                RelationId = RelationId,
                Weight = Weight,
                SourceTableId = SourceTableId,
                TargetTableId = TargetTableId,
                SourceTable = SourceTable,
                TargetTable = TargetTable,
                SourceColumnName = SourceColumnName,
                TargetColumnName = TargetColumnName
            };
        }

        public GraphEdge Reverse()
        {
            var x = Clone();
            x.RelationId = -x.RelationId;
            (x.SourceTableId, x.TargetTableId) = (x.TargetTableId, x.SourceTableId);
            (x.SourceTable, x.TargetTable) = (x.TargetTable, x.SourceTable);
            (x.SourceColumnName, x.TargetColumnName) = (x.TargetColumnName, x.SourceColumnName);
            return x;
        }

        public GraphEdge makeAlias(GraphNode alias)
        {
            var x = Clone();
            x.SourceTable = alias;
            x.RelationId = 1000 + RelationId;
            return x;
        }

        public Tuple<string, string> Key { get { return new Tuple<string, string>(SourceTableName, TargetTableName); } }
    }

}
