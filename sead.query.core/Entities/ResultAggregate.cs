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
        public bool IsAggregateField { get; set; }
        public bool IsItemField { get; set; }
        public string SqlFieldCompiler { get; set; }
        public string SqlTemplate { get; set; }

        public bool IsGroupByField => IsItemField || IsSortField;

        private ISqlFieldCompiler __compiler = null;
        public ISqlFieldCompiler Compiler => __compiler ?? (__compiler = CreateCompiler());

        private ISqlFieldCompiler CreateCompiler()
        {
            return (ISqlFieldCompiler)Activator.CreateInstance(Type.GetType($"SeadQueryCore.{SqlFieldCompiler}"), this);
        }
    }

    public class ResultAggregate {
        public int AggregateId { get; set; }
        public string AggregateKey { get; set; }
        public string DisplayText { get; set; }
        public bool IsApplicable { get; set; }
        public bool IsActivated { get; set; }
        public string InputType { get; set; }
        public bool HasSelector { get; set; }

        public List<ResultAggregateField> Fields { get; set; }

        public List<ResultAggregateField> GetResultFields()
            => GetFields().Where(z => z.FieldType.IsResultValue).OrderBy(z => z.SequenceId).ToList();

        public List<ResultAggregateField> GetFields()
            => Fields.OrderBy(z => z.SequenceId).ToList();
    }

    public class ResultAggregateField {
        public int AggregateFieldId { get; set; }
        public int SequenceId { get; set; }

        public int AggregateId { get; set; }
        public ResultAggregate Aggregate { get; set; }

        public int ResultFieldId { get; set; }
        public ResultField ResultField { get; set; }

        public string FieldTypeId { get; set; }
        public ResultFieldType FieldType { get; set; }
    }

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

        public ResultFieldType FieldType { get; set; }
    }
}
