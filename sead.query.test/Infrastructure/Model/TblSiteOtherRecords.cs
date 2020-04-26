using System;
using System.Collections.Generic;

namespace SQT.Infrastructure.Model
{
    public partial class TblSiteOtherRecords
    {
        public int SiteOtherRecordsId { get; set; }
        public int? SiteId { get; set; }
        public int? BiblioId { get; set; }
        public int? RecordTypeId { get; set; }
        public string Description { get; set; }
        public DateTime? DateUpdated { get; set; }

        public virtual TblBiblio Biblio { get; set; }
        public virtual TblRecordTypes RecordType { get; set; }
        public virtual TblSites Site { get; set; }
    }
}
