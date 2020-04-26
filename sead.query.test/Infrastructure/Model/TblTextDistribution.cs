using System;
using System.Collections.Generic;

namespace SQT.Infrastructure.Model
{
    public partial class TblTextDistribution
    {
        public int DistributionId { get; set; }
        public int BiblioId { get; set; }
        public DateTime? DateUpdated { get; set; }
        public string DistributionText { get; set; }
        public int TaxonId { get; set; }

        public virtual TblBiblio Biblio { get; set; }
        public virtual TblTaxaTreeMaster Taxon { get; set; }
    }
}
