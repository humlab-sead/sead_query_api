using System;
using System.Collections.Generic;

namespace SeadQueryTest.Models
{
    public partial class ResultViewType
    {
        public string ViewTypeId { get; set; }
        public string ViewName { get; set; }
        public bool? IsCachable { get; set; }
    }
}
