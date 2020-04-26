using System;
using System.Collections.Generic;

namespace SQT.Infrastructure.Model
{
    public partial class TblTaxaSynonyms
    {
        public int SynonymId { get; set; }
        public int BiblioId { get; set; }
        public DateTime? DateUpdated { get; set; }
        public int? FamilyId { get; set; }
        public int? GenusId { get; set; }
        public string Notes { get; set; }
        public int? TaxonId { get; set; }
        public int? AuthorId { get; set; }
        public string Synonym { get; set; }
        public string ReferenceType { get; set; }

        public virtual TblTaxaTreeAuthors Author { get; set; }
        public virtual TblBiblio Biblio { get; set; }
        public virtual TblTaxaTreeFamilies Family { get; set; }
        public virtual TblTaxaTreeGenera Genus { get; set; }
        public virtual TblTaxaTreeMaster Taxon { get; set; }
    }
}
