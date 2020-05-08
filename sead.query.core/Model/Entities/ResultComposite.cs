using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace SeadQueryCore
{

    public class ResultComposite {
        public ResultComposite()
        {
            Fields = new HashSet<ResultCompositeField>();
        }

        public int CompositeId { get; set; }
        public string CompositeKey { get; set; }
        public string DisplayText { get; set; }
        public bool IsActivated { get; set; }

        [JsonIgnore]
        public virtual ICollection<ResultCompositeField> Fields { get; set; }

        public IEnumerable<ResultCompositeField> GetSortedFields()
            => Fields.OrderBy(z => z.SequenceId);

        public IEnumerable<ResultCompositeField> GetResultFields()
            => GetSortedFields()
                .Where(z => z.FieldType.IsResultValue);
 
        public IEnumerable<(string Name, Type Type)> GetResultFieldTypes()
        {
            return GetResultFields()
                .Select((field, i) => ($"alias_{i + 1}", field.ResultField.GetDataType()));
        }

    }
}
