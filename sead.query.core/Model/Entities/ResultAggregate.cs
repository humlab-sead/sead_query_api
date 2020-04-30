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
        public bool IsActivated { get; set; }

        [JsonIgnore]
        public virtual ICollection<ResultAggregateField> Fields { get; set; }

        public IEnumerable<ResultAggregateField> GetSortedFields()
            => Fields.OrderBy(z => z.SequenceId);

        public IEnumerable<ResultAggregateField> GetResultFields()
            => GetSortedFields()
                .Where(z => z.FieldType.IsResultValue);
 
        public IEnumerable<(string Name, Type Type)> GetResultFieldTypes()
        {
            return GetResultFields()
                .Select((field, i) => ($"alias_{i + 1}", field.ResultField.GetDataType()));
        }

    }
}
