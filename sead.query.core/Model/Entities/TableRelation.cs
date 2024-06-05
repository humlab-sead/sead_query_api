﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SeadQueryCore
{
    public interface IGraphEdge
    {
        public int SourceId { get; }
        public int TargetId { get; }
        public int Weight { get; }

        Tuple<string, string> Key { get; }
        Tuple<int, int> IdKey { get; }

        IGraphEdge Reverse();
    }

    public class TableRelation : IGraphEdge
    {
        public int TableRelationId { get; set; }
        public int SourceTableId { get; set; }
        public int TargetTableId { get; set; }
        public int Weight { get; set; }
        //public string ExtraConstraint { get; set; }

        public string SourceColumName { get; set; }
        public string TargetColumnName { get; set; }

        public int SourceId { get { return SourceTableId; } }
        public int TargetId { get { return TargetTableId; } }

        [JsonIgnore] private Table _SourceTable, _TargetTable;

        public Table SourceTable { get { return _SourceTable; } set { _SourceTable = value; SourceTableId = value?.TableId ?? SourceTableId; } }
        public Table TargetTable { get { return _TargetTable; } set { _TargetTable = value; TargetTableId = value?.TableId ?? TargetTableId; } }

        [JsonIgnore] public string SourceName { get { return SourceTable?.TableOrUdfName ?? ""; } }
        [JsonIgnore] public string TargetName { get { return TargetTable?.TableOrUdfName ?? ""; } }

        public TableRelation Clone()
        {
            return new TableRelation()
            {
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

        public IGraphEdge Reverse()
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

        public Tuple<string, string> Key => new(SourceName, TargetName);
        public Tuple<int, int> IdKey => new(SourceTableId, TargetTableId);

        public bool IsOf(TableRelation x)
        {
            return x != null && IsOf(x.SourceName, x.TargetName);
        }

        public bool IsOf(string sourceName, string targetName)
        {
            return SourceName == sourceName && TargetName == targetName;
        }

        public override int GetHashCode()
        {
            return Key.GetHashCode();
        }


        public override bool Equals(object x)
        {
            return IsOf(x as TableRelation);
        }

        public bool InvolvesAny(int[] tableIds)
        {
            return tableIds.Contains(SourceTableId) || tableIds.Contains(TargetTableId);
        }

        public bool IsGated()
        {
            return SourceTable.IsGated || TargetTable.IsGated;
        }
    }
}
