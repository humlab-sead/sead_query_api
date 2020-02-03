using System;
using System.Collections.Generic;

namespace SeadQueryTest.Models
{
    public partial class TableRelation
    {
        public int TableRelationId { get; set; }
        public int SourceTableId { get; set; }
        public int TargetTableId { get; set; }
        public int Weight { get; set; }
        public string SourceColumnName { get; set; }
        public string TargetColumnName { get; set; }

        public virtual Table SourceTable { get; set; }
        public virtual Table TargetTable { get; set; }
    }
}
