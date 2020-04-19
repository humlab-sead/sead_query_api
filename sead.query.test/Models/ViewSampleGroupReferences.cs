using System;
using System.Collections.Generic;

namespace SQT.Models
{
    public partial class ViewSampleGroupReferences
    {
        public int? SampleGroupId { get; set; }
        public int? BiblioId { get; set; }
        public DateTime? DateUpdated { get; set; }
        public string BiblioLink { get; set; }
    }
}
