using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace SeadQueryCore
{

    public class ResultAggregate {
        public ResultAggregate()
        {
            Fields = new HashSet<ResultAggregateField>();
        }

        public int AggregateId { get; set; }
        public string AggregateKey { get; set; }
        public string DisplayText { get; set; }
        public bool IsApplicable { get; set; }
        public bool IsActivated { get; set; }
        public string InputType { get; set; }
        public bool HasSelector { get; set; }

        public virtual ICollection<ResultAggregateField> Fields { get; set; }

        public List<ResultAggregateField> GetResultFields()
            => GetFields().Where(z => z.FieldType.IsResultValue).OrderBy(z => z.SequenceId).ToList();

        public List<ResultAggregateField> GetFields()
            => Fields.OrderBy(z => z.SequenceId).ToList();

    }
}
