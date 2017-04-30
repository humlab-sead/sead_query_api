using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace QueryFacetDomain
{

    public enum EFacetType {
        Unknown = 0,
        Discrete = 1,
        Range = 2,
        Geo = 3
    }

    public class FacetType {

        public EFacetType FacetTypeId { get; set; }
        public string FacetTypeName { get; set; }

    }

    public class FacetGroup {

        public int FacetGroupId { get; set; }
        public string FacetGroupKey { get; set; }
        public string DisplayTitle { get; set; }
        public bool IsApplicable { get; set; }
        public bool IsDefault { get; set; }

    }

    public class FacetConditionClause {

        public int FacetSourceTableId { get; set; }
        public int FacetId { get; set; }
        public string Clause { get; set; }

        public FacetDefinition FacetDefinition { get; set; }
    }

    // FIXME facet.facet_table should perhaps instead reference facet.graph_table i.e. replace tablename with a table_id FK.
    public class FacetTable {

        public int FacetTableId { get; set; }
        public int FacetId { get; set; }
        public int SequenceId { get; set; }
        public string SchemaName { get; set; }
        public string TableName { get; set; }
        public string Alias { get; set; }

        public FacetDefinition FacetDefinition { get; set; }
    }

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

        public Tuple<string, string> Key { get { return new Tuple<string, string>(SourceTableName, TargetTableName);  } }
    }

    public class FacetDefinition {

        public int FacetId { get; set; }
        public string FacetCode { get; set; }
        public string DisplayTitle { get; set; }
        public int FacetGroupId { get; set; }
        public EFacetType FacetTypeId { get; set; }
        public string CategoryIdExpr { get; set; }
        public string CategoryNameExpr { get; set; }
        public string IconIdExpr { get; set; }
        public string SortExpr { get; set; }
        public bool IsApplicable { get; set; }
        public bool IsDefault { get; set; }
        public string AggregateType { get; set; }
        public string AggregateTitle { get; set; }
        public int AggregateFacetId { get; set; }

        public FacetType FacetType { get; set; }
        public FacetGroup FacetGroup { get; set; }
        public List<FacetTable> Tables { get; set; } // = new List<FacetTable>();
        public List<FacetConditionClause> Clauses { get; set; } // = new List<FacetConditionClause>();

        public FacetTable TargetTable       => Tables.Find(z => z.SequenceId == 1);
        public string TargetTableName       => TargetTable.TableName;
        public List<FacetTable> ExtraTables => Tables.Where(z => z.SequenceId != 1).ToList();

        public string aliasName = null;
        public string AliasName {
            get {
                return aliasName ?? (aliasName = Tables.FirstOrDefault(z => !z.Alias.Equals(""))?.Alias ?? "");
            }
        }

        public string ResolvedName  => AliasName != "" ? AliasName : TargetTableName; 
        public string QueryCriteria => String.Join(" AND ", Clauses.Select(x => x.Clause));

    }

    public enum EResultType {
        single_item = 1,
        text_agg_item = 2,
        count_item = 3,
        link_item = 4,
        sort_item = 5,
        link_item_filtered = 6
    }

    public class ResultType {
        public EResultType ResultTypeId { get; set; }
        public string ResultTypeName { get; set; }
    }

    public class ResultDefinition {

        public int ResultDefinitionId { get; set; }
        public string Key { get; set; }
        public string DisplayText { get; set; }
        public bool IsApplicable { get; set; }
        public bool IsActivated { get; set; }
        public string AggregationType { get; set; }
        public string InputType { get; set; }
        public bool HasAggregationSelector { get; set; }

        public List<ResultDefinitionField> Fields { get; set; }
    }

    public class ResultDefinitionField {

        public int ResultDefinitionFieldId { get; set; }
        public int ResultDefinitionId { get; set; }
        public int ResultFieldId { get; set; }
        public EResultType ResultTypeId { get; set; }

        public ResultDefinition ResultDefinition { get; set; }
        public ResultField ResultField { get; set; }
        public ResultType ResultType { get; set; }
    }

    public class ResultField {

        public int ResultFieldId { get; set; }
        public string ResultFieldKey { get; set; }
        public string TableName { get; set; }
        public string ColumnName { get; set; }
        public string DisplayText { get; set; }
        public string ResultType { get; set; }
        public bool Activated { get; set; }
        public string LinkUrl { get; set; }
        public string LinkLabel { get; set; }
    }
}
