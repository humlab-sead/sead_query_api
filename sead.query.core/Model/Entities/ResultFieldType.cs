using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace SeadQueryCore
{
    public class ResultFieldType
    {
        public string FieldTypeId { get; set; }
        public bool IsResultValue { get; set; }
        public bool IsSortField { get; set; }
        public virtual bool IsAggregateField { get; set; }
        public bool IsItemField { get; set; }
        public string SqlFieldCompiler { get; set; }
        public virtual string SqlTemplate { get; set; }

        public bool IsGroupByField => IsItemField || IsSortField;

        private ISqlFieldCompiler __compiler = null;

        [JsonIgnore]
        public ISqlFieldCompiler Compiler => __compiler ?? (__compiler = CreateCompiler());

        private ISqlFieldCompiler CreateCompiler()
        {
            return (ISqlFieldCompiler)Activator.CreateInstance(Type.GetType($"SeadQueryCore.{SqlFieldCompiler}"), this);
        }
    }
}
