using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace sead.query.test.data.Entities
{
    public partial class ResultAggregateField
    {
        public int AggregateFieldId { get; set; }
        public int AggregateId { get; set; }
        public int ResultFieldId { get; set; }
        public string FieldTypeId { get; set; }
        public int SequenceId { get; set; }

        [JsonIgnore]
        public virtual ResultAggregate Aggregate { get; set; }
        [JsonIgnore]
        public virtual ResultFieldType FieldType { get; set; }
        [JsonIgnore]
        public virtual ResultField ResultField { get; set; }
    }
}
