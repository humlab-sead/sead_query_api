using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace SeadQueryCore
{

    public class ResultCompositeField {
        public int CompositeFieldId { get; set; }
        public int CompositeId { get; set; }
        public string FieldTypeId { get; set; }
        public int ResultFieldId { get; set; }
        public int SequenceId { get; set; }

        public virtual ResultComposite Composite { get; set; }
        public virtual ResultFieldType FieldType { get; set; }
        public virtual ResultField ResultField { get; set; }
    }
}
