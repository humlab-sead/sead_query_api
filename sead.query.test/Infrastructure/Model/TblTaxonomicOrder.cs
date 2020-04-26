using System;
using System.Collections.Generic;

namespace SQT.Infrastructure.Model
{
    public partial class TblTaxonomicOrder
    {
        public int TaxonomicOrderId { get; set; }
        public DateTime? DateUpdated { get; set; }
        public int? TaxonId { get; set; }
        public decimal? TaxonomicCode { get; set; }
        public int? TaxonomicOrderSystemId { get; set; }

        public virtual TblTaxaTreeMaster Taxon { get; set; }
        public virtual TblTaxonomicOrderSystems TaxonomicOrderSystem { get; set; }
    }
}
