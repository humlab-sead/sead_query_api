using System;
using System.Collections.Generic;

namespace SQT.Infrastructure.Model
{
    public partial class TblTextIdentificationKeys
    {
        public int KeyId { get; set; }
        public int BiblioId { get; set; }
        public DateTime? DateUpdated { get; set; }
        public string KeyText { get; set; }
        public int TaxonId { get; set; }

        public virtual TblBiblio Biblio { get; set; }
        public virtual TblTaxaTreeMaster Taxon { get; set; }
    }
}
