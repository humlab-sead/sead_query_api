using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace QuerySeadDomain
{
    public enum EResultType {
        single_item = 1,
        text_agg_item = 2,
        count_item = 3,
        link_item = 4,
        sort_item = 5,
        link_item_filtered = 6
    }

    public class ResultType {
        public EResultType ResultTypeId { get; set; }
        public string ResultTypeName { get; set; }
    }

    public class ResultDefinition {

        public int ResultDefinitionId { get; set; }
        public string Key { get; set; }
        public string DisplayText { get; set; }
        public bool IsApplicable { get; set; }
        public bool IsActivated { get; set; }
        public string AggregationType { get; set; }
        public string InputType { get; set; }
        public bool HasAggregationSelector { get; set; }

        public List<ResultDefinitionField> Fields { get; set; }
    }

    public class ResultDefinitionField {

        public int ResultDefinitionFieldId { get; set; }
        public int ResultDefinitionId { get; set; }
        public int ResultFieldId { get; set; }
        public EResultType ResultTypeId { get; set; }

        public ResultDefinition ResultDefinition { get; set; }
        public ResultField ResultField { get; set; }
        public ResultType ResultType { get; set; }
    }

    public class ResultField {

        public int ResultFieldId { get; set; }
        public string ResultFieldKey { get; set; }
        public string TableName { get; set; }
        public string ColumnName { get; set; }
        public string DisplayText { get; set; }
        public string ResultType { get; set; }
        public bool Activated { get; set; }
        public string LinkUrl { get; set; }
        public string LinkLabel { get; set; }

    }
}
