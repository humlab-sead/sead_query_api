using System;
using System.Collections.Generic;

namespace SQT.Infrastructure.Model
{
    public partial class TblTaxaTreeAuthors
    {
        public TblTaxaTreeAuthors()
        {
            TblTaxaSynonyms = new HashSet<TblTaxaSynonyms>();
            TblTaxaTreeMaster = new HashSet<TblTaxaTreeMaster>();
        }

        public int AuthorId { get; set; }
        public string AuthorName { get; set; }
        public DateTime? DateUpdated { get; set; }

        public virtual ICollection<TblTaxaSynonyms> TblTaxaSynonyms { get; set; }
        public virtual ICollection<TblTaxaTreeMaster> TblTaxaTreeMaster { get; set; }
    }
}
