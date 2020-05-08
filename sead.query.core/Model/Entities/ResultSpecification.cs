using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace SeadQueryCore
{

    public class ResultSpecification {
        public ResultSpecification()
        {
            Fields = new HashSet<ResultSpecificationField>();
        }

        public int SpecificationId { get; set; }
        public string SpecificationKey { get; set; }
        public string DisplayText { get; set; }
        public bool IsActivated { get; set; }

        [JsonIgnore]
        public virtual ICollection<ResultSpecificationField> Fields { get; set; }

        public IEnumerable<ResultSpecificationField> GetSortedFields()
            => Fields.OrderBy(z => z.SequenceId);

        public IEnumerable<ResultSpecificationField> GetResultFields()
            => GetSortedFields()
                .Where(z => z.FieldType.IsResultValue);
 
        public IEnumerable<(string Name, Type Type)> GetResultFieldTypes()
        {
            return GetResultFields()
                .Select((field, i) => ($"alias_{i + 1}", field.ResultField.GetDataType()));
        }

    }
}
