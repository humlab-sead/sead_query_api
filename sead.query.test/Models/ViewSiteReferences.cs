using System;
using System.Collections.Generic;

namespace SQT.Models
{
    public partial class ViewSiteReferences
    {
        public int? SiteId { get; set; }
        public int? BiblioId { get; set; }
        public DateTime? DateUpdated { get; set; }
        public string BiblioLink { get; set; }
    }
}
