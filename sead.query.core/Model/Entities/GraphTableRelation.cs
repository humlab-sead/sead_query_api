using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SeadQueryCore
{

    public class GraphTableRelation {
        public int RelationId { get; set; }
        public int SourceTableId { get; set; }
        public int TargetTableId { get; set; }
        public int Weight { get; set; }

        public string SourceColumnName { get; set; }
        public string TargetColumnName { get; set; }

        [JsonIgnore] private GraphTable _SourceTable, _TargetTable;

        public GraphTable SourceTable { get { return _SourceTable; } set { _SourceTable = value; SourceTableId = value?.NodeId ?? SourceTableId;  } }
        public GraphTable TargetTable { get { return _TargetTable; } set { _TargetTable = value; TargetTableId = value?.NodeId ?? TargetTableId; } }

        [JsonIgnore] public string SourceTableName { get { return SourceTable.TableName; } }
        [JsonIgnore] public string TargetTableName { get { return TargetTable.TableName; } }

        public GraphTableRelation Clone()
        {
            return new GraphTableRelation() {
                RelationId = RelationId + 1000,
                Weight = Weight,
                SourceTableId = SourceTableId,
                TargetTableId = TargetTableId,
                SourceTable = SourceTable,
                TargetTable = TargetTable,
                SourceColumnName = SourceColumnName,
                TargetColumnName = TargetColumnName
            };
        }

        public GraphTableRelation Reverse()
        {
            var x = Clone();
            x.RelationId = -x.RelationId;
            (x.SourceTableId, x.TargetTableId) = (x.TargetTableId, x.SourceTableId);
            (x.SourceTable, x.TargetTable) = (x.TargetTable, x.SourceTable);
            (x.SourceColumnName, x.TargetColumnName) = (x.TargetColumnName, x.SourceColumnName);
            return x;
        }

        public GraphTableRelation Alias(GraphTable node, GraphTable alias)
        {
            var x = Clone();
            if (node.NodeId == SourceTable.NodeId)
                x.SourceTable = alias;
            else
                x.TargetTable = alias;
            return x;
        }

        public Tuple<string, string> Key { get { return new Tuple<string, string>(SourceTableName, TargetTableName); } }

        public bool EqualAs(GraphTableRelation x)
        {
            //return (SourceTableId == x.SourceTableId) && (TargetTableId == x.TargetTableId);
            return (SourceTableName == x.SourceTableName) && (TargetTableName == x.TargetTableName);
        }

        public string ToStringPair()
        {
            return $"{SourceTableName}/{TargetTableName}";
        }
    }
}
