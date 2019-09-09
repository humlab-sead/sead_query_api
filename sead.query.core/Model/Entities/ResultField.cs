using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace SeadQueryCore
{

    public class ResultField {
        [JsonIgnore] public int ResultFieldId { get; set; }
        [JsonIgnore] public string TableName { get; set; }
        [JsonIgnore] public string ColumnName { get; set; }
        public string ResultFieldKey { get; set; }
        public string DisplayText { get; set; }
        public string FieldTypeId { get; set; }
        public bool Activated { get; set; }
        public string LinkUrl { get; set; }
        public string LinkLabel { get; set; }

        public virtual ResultFieldType FieldType { get; set; }
    }
}
