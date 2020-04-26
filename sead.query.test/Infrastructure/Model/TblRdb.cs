using System;
using System.Collections.Generic;

namespace SQT.Infrastructure.Model
{
    public partial class TblRdb
    {
        public int RdbId { get; set; }
        public int LocationId { get; set; }
        public int? RdbCodeId { get; set; }
        public int TaxonId { get; set; }
        public DateTime? DateUpdated { get; set; }

        public virtual TblLocations Location { get; set; }
        public virtual TblRdbCodes RdbCode { get; set; }
        public virtual TblTaxaTreeMaster Taxon { get; set; }
    }
}
