using System;
using System.Collections.Generic;

namespace SeadQueryTest.Models
{
    public partial class ResultField
    {
        public ResultField()
        {
            ResultAggregateField = new HashSet<ResultAggregateField>();
        }

        public int ResultFieldId { get; set; }
        public string ResultFieldKey { get; set; }
        public string TableName { get; set; }
        public string ColumnName { get; set; }
        public string DisplayText { get; set; }
        public string FieldTypeId { get; set; }
        public bool Activated { get; set; }
        public string LinkUrl { get; set; }
        public string LinkLabel { get; set; }
        public string DataType { get; set; }

        public virtual ResultFieldType FieldType { get; set; }
        public virtual Table TableNameNavigation { get; set; }
        public virtual ICollection<ResultAggregateField> ResultAggregateField { get; set; }
    }
}
