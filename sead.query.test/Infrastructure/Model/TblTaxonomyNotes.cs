using System;
using System.Collections.Generic;

namespace SQT.Infrastructure.Model
{
    public partial class TblTaxonomyNotes
    {
        public int TaxonomyNotesId { get; set; }
        public int BiblioId { get; set; }
        public DateTime? DateUpdated { get; set; }
        public int TaxonId { get; set; }
        public string TaxonomyNotes { get; set; }

        public virtual TblBiblio Biblio { get; set; }
        public virtual TblTaxaTreeMaster Taxon { get; set; }
    }
}
