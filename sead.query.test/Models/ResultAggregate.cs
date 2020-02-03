using System;
using System.Collections.Generic;

namespace SeadQueryTest.Models
{
    public partial class ResultAggregate
    {
        public ResultAggregate()
        {
            ResultAggregateField = new HashSet<ResultAggregateField>();
        }

        public int AggregateId { get; set; }
        public string AggregateKey { get; set; }
        public string DisplayText { get; set; }
        public bool IsApplicable { get; set; }
        public bool? IsActivated { get; set; }
        public string InputType { get; set; }
        public bool? HasSelector { get; set; }

        public virtual ICollection<ResultAggregateField> ResultAggregateField { get; set; }
    }
}
