using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace sead.query.test.data.Entities
{
    public partial class ResultAggregate
    {
        public ResultAggregate()
        {
            Fields = new HashSet<ResultAggregateField>();
        }

        public int AggregateId { get; set; }
        public string AggregateKey { get; set; }
        public string DisplayText { get; set; }
        public bool IsApplicable { get; set; }
        public bool? IsActivated { get; set; }
        public string InputType { get; set; }
        public bool? HasSelector { get; set; }

        [JsonIgnore]
        public virtual ICollection<ResultAggregateField> Fields { get; set; }
    }
}
