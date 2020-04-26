using System;
using System.Collections.Generic;

namespace SQT.Infrastructure.Model
{
    public partial class TblTaxaCommonNames
    {
        public int TaxonCommonNameId { get; set; }
        public string CommonName { get; set; }
        public DateTime? DateUpdated { get; set; }
        public int? LanguageId { get; set; }
        public int? TaxonId { get; set; }

        public virtual TblLanguages Language { get; set; }
        public virtual TblTaxaTreeMaster Taxon { get; set; }
    }
}
