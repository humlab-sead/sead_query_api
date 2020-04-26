using System;
using System.Collections.Generic;

namespace SQT.Infrastructure.Model
{
    public partial class TblSiteReferences
    {
        public int SiteReferenceId { get; set; }
        public int? SiteId { get; set; }
        public int BiblioId { get; set; }
        public DateTime? DateUpdated { get; set; }

        public virtual TblBiblio Biblio { get; set; }
        public virtual TblSites Site { get; set; }
    }
}
