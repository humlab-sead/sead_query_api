using System;
using System.Collections.Generic;

namespace SQT.Infrastructure.Model
{
    public partial class TblMcrNames
    {
        public int TaxonId { get; set; }
        public string ComparisonNotes { get; set; }
        public DateTime? DateUpdated { get; set; }
        public string McrNameTrim { get; set; }
        public short? McrNumber { get; set; }
        public string McrSpeciesName { get; set; }

        public virtual TblTaxaTreeMaster Taxon { get; set; }
    }
}
