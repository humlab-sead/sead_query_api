using System;
using System.Collections.Generic;

namespace SQT.Models
{
    public partial class Table
    {
        public Table()
        {
            FacetTable = new HashSet<FacetTable>();
            ResultField = new HashSet<ResultField>();
            TableRelationSourceTable = new HashSet<TableRelation>();
            TableRelationTargetTable = new HashSet<TableRelation>();
        }

        public int TableId { get; set; }
        public string SchemaName { get; set; }
        public string TableOrUdfName { get; set; }
        public string PrimaryKeyName { get; set; }
        public bool IsUdf { get; set; }

        public virtual ICollection<FacetTable> FacetTable { get; set; }
        public virtual ICollection<ResultField> ResultField { get; set; }
        public virtual ICollection<TableRelation> TableRelationSourceTable { get; set; }
        public virtual ICollection<TableRelation> TableRelationTargetTable { get; set; }
    }
}
