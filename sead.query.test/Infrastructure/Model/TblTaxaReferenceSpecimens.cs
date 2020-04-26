using System;
using System.Collections.Generic;

namespace SQT.Infrastructure.Model
{
    public partial class TblTaxaReferenceSpecimens
    {
        public int TaxaReferenceSpecimenId { get; set; }
        public int TaxonId { get; set; }
        public int ContactId { get; set; }
        public string Notes { get; set; }
        public DateTime? DateUpdated { get; set; }

        public virtual TblContacts Contact { get; set; }
        public virtual TblTaxaTreeMaster Taxon { get; set; }
    }
}
