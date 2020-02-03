using System;
using System.Collections.Generic;

namespace SeadQueryTest.Models
{
    public partial class ResultFieldType
    {
        public ResultFieldType()
        {
            ResultAggregateField = new HashSet<ResultAggregateField>();
            ResultField = new HashSet<ResultField>();
        }

        public string FieldTypeId { get; set; }
        public bool? IsResultValue { get; set; }
        public string SqlFieldCompiler { get; set; }
        public bool IsAggregateField { get; set; }
        public bool IsSortField { get; set; }
        public bool IsItemField { get; set; }
        public string SqlTemplate { get; set; }

        public virtual ICollection<ResultAggregateField> ResultAggregateField { get; set; }
        public virtual ICollection<ResultField> ResultField { get; set; }
    }
}
