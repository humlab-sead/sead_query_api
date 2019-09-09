using System;
using System.Collections.Generic;

namespace sead.query.test.data.Entities
{
    public partial class ResultViewType
    {
        public string ViewTypeId { get; set; }
        public string ViewName { get; set; }
        public bool? IsCachable { get; set; }
    }
}
