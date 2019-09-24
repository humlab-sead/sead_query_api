using System;
using System.Collections.Generic;

namespace sead.query.test.data.Entities
{
    public partial class ResultFieldType
    {
        public string FieldTypeId { get; set; }
        public bool? IsResultValue { get; set; }
        public string SqlFieldCompiler { get; set; }
        public bool IsAggregateField { get; set; }
        public bool IsSortField { get; set; }
        public bool IsItemField { get; set; }
        public string SqlTemplate { get; set; }

    }
}
