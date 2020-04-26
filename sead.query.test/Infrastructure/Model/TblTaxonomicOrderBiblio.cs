using System;
using System.Collections.Generic;

namespace SQT.Infrastructure.Model
{
    public partial class TblTaxonomicOrderBiblio
    {
        public int TaxonomicOrderBiblioId { get; set; }
        public int? BiblioId { get; set; }
        public DateTime? DateUpdated { get; set; }
        public int? TaxonomicOrderSystemId { get; set; }

        public virtual TblBiblio Biblio { get; set; }
        public virtual TblTaxonomicOrderSystems TaxonomicOrderSystem { get; set; }
    }
}
