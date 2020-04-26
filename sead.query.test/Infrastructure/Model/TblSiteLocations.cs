using System;
using System.Collections.Generic;

namespace SQT.Infrastructure.Model
{
    public partial class TblSiteLocations
    {
        public int SiteLocationId { get; set; }
        public DateTime? DateUpdated { get; set; }
        public int LocationId { get; set; }
        public int SiteId { get; set; }

        public virtual TblLocations Location { get; set; }
        public virtual TblSites Site { get; set; }
    }
}
