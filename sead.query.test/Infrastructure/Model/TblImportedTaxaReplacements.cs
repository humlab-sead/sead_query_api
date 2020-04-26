using System;
using System.Collections.Generic;

namespace SQT.Infrastructure.Model
{
    public partial class TblImportedTaxaReplacements
    {
        public int ImportedTaxaReplacementId { get; set; }
        public DateTime? DateUpdated { get; set; }
        public string ImportedNameReplaced { get; set; }
        public int TaxonId { get; set; }

        public virtual TblTaxaTreeMaster Taxon { get; set; }
    }
}
