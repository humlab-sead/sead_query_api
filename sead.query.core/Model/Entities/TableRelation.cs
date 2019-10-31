using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SeadQueryCore
{

    public class TableRelation {
        public int TableRelationId { get; set; }
        public int SourceTableId { get; set; }
        public int TargetTableId { get; set; }
        public int Weight { get; set; }

        public string SourceColumName { get; set; }
        public string TargetColumnName { get; set; }

        [JsonIgnore] private Table _SourceTable, _TargetTable;

        public Table SourceTable { get { return _SourceTable; } set { _SourceTable = value; SourceTableId = value?.TableId ?? SourceTableId;  } }
        public Table TargetTable { get { return _TargetTable; } set { _TargetTable = value; TargetTableId = value?.TableId ?? TargetTableId; } }

        [JsonIgnore] public string SourceName { get { return SourceTable?.TableOrUdfName ?? ""; } }
        [JsonIgnore] public string TargetName { get { return TargetTable?.TableOrUdfName ?? ""; } }

        public TableRelation Clone()
        {
            return new TableRelation() {
                TableRelationId = TableRelationId + 1000,
                Weight = Weight,
                SourceTableId = SourceTableId,
                TargetTableId = TargetTableId,
                SourceTable = SourceTable,
                TargetTable = TargetTable,
                SourceColumName = SourceColumName,
                TargetColumnName = TargetColumnName
            };
        }

        public TableRelation Reverse()
        {
            var x = Clone();
            x.TableRelationId = -x.TableRelationId;
            (x.SourceTableId, x.TargetTableId) = (x.TargetTableId, x.SourceTableId);
            (x.SourceTable, x.TargetTable) = (x.TargetTable, x.SourceTable);
            (x.SourceColumName, x.TargetColumnName) = (x.TargetColumnName, x.SourceColumName);
            return x;
        }

        public TableRelation Alias(Table node, Table alias)
        {
            var x = Clone();
            if (node.TableId == SourceTable.TableId)
                x.SourceTable = alias;
            else
                x.TargetTable = alias;
            return x;
        }

        public Tuple<string, string> Key { get { return new Tuple<string, string>(SourceName, TargetName); } }

        public override bool Equals(object x)
        {
            return Equal(x as TableRelation);
        }

        public bool Equal(TableRelation x)
        {
            return x != null && (SourceName == x.SourceName) && (TargetName == x.TargetName);
        }

        public override int GetHashCode()
        {
            return this.Key.GetHashCode();
        }

        public string ToStringPair()
        {
            return $"{SourceName}/{TargetName}";
        }
    }
}
