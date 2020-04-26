using System;
using System.Collections.Generic;

namespace SQT.Infrastructure.Model
{
    public partial class TblMcrSummaryData
    {
        public int McrSummaryDataId { get; set; }
        public short? CogMidTmax { get; set; }
        public short? CogMidTrange { get; set; }
        public DateTime? DateUpdated { get; set; }
        public int TaxonId { get; set; }
        public short? TmaxHi { get; set; }
        public short? TmaxLo { get; set; }
        public short? TminHi { get; set; }
        public short? TminLo { get; set; }
        public short? TrangeHi { get; set; }
        public short? TrangeLo { get; set; }

        public virtual TblTaxaTreeMaster Taxon { get; set; }
    }
}
