using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace sead.query.test.data.Entities
{
    public partial class ResultField
    {
        public int ResultFieldId { get; set; }
        public string ResultFieldKey { get; set; }
        public string TableName { get; set; }
        public string ColumnName { get; set; }
        public string DisplayText { get; set; }
        public string FieldTypeId { get; set; }
        public bool Activated { get; set; }
        public string LinkUrl { get; set; }
        public string LinkLabel { get; set; }

        [JsonIgnore]
        public virtual ResultFieldType FieldType { get; set; }
    }
}
