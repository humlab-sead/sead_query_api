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
        public string DataType { get; set; }

        public virtual ResultFieldType FieldType { get; set; }

        public virtual Type GetDataType()
        {
            if ("text string varchar".Contains(DataType)) {
                return typeof(string);
            }
            if ("numeric decimal".Contains(DataType)) {
                return typeof(decimal);
            }
            if ("bool boolean".Contains(DataType)) {
                return typeof(bool);
            }
            if ("int integer smallint short long".Contains(DataType)) {
                return typeof(int);
            }
            if ("float".Contains(DataType)) {
                return typeof(float);
            }
            throw new ArgumentException($"Unknown field type {DataType}");
        }
    }
}
