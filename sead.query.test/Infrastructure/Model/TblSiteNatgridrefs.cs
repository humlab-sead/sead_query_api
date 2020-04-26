using System;
using System.Collections.Generic;

namespace SQT.Infrastructure.Model
{
    public partial class TblSiteNatgridrefs
    {
        public int SiteNatgridrefId { get; set; }
        public int SiteId { get; set; }
        public int MethodId { get; set; }
        public string Natgridref { get; set; }
        public DateTime? DateUpdated { get; set; }

        public virtual TblMethods Method { get; set; }
        public virtual TblSites Site { get; set; }
    }
}
