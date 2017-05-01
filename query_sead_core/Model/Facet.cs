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
    
}
