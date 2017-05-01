using System;
using System.Collections.Generic;
using System.Text;

namespace QueryFacetDomain
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
        public string SourceTableName { get { return SourceTable.TableName; } }
        public string TargetTableName { get { return TargetTable.TableName; } }

        public string SourceName { get { return $"{SourceTableName}.\"{SourceColumnName}\""; } }
        public string TargetName { get { return $"{TargetTableName}.\"{TargetColumnName}\""; } }

        // Note! Only simple, non-composite key allows (just id-2-id), and for now no extra critera
        public string GetSqlJoinClause(string joinType)
            => $" INNER JOIN {TargetTableName} ON ({SourceName} = {TargetName})\n";

        public GraphEdge Clone()
        {
            return new GraphEdge() {
                RelationId = RelationId,
                Weight = Weight,
                SourceTableId = SourceTableId,
                TargetTableId = TargetTableId,
                SourceTable = SourceTable,
                TargetTable = TargetTable,
            };
        }

        public GraphEdge Reverse()
        {
            var x = Clone();
            x.RelationId = -x.RelationId;
            (x.SourceTableId, x.TargetTableId) = (x.TargetTableId, x.SourceTableId);
            (x.SourceTable, x.TargetTable) = (x.TargetTable, x.SourceTable);
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
