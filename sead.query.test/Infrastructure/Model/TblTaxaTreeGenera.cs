using System;
using System.Collections.Generic;

namespace SQT.Infrastructure.Model
{
    public partial class TblTaxaTreeGenera
    {
        public TblTaxaTreeGenera()
        {
            TblTaxaSynonyms = new HashSet<TblTaxaSynonyms>();
            TblTaxaTreeMaster = new HashSet<TblTaxaTreeMaster>();
        }

        public int GenusId { get; set; }
        public DateTime? DateUpdated { get; set; }
        public int? FamilyId { get; set; }
        public string GenusName { get; set; }

        public virtual TblTaxaTreeFamilies Family { get; set; }
        public virtual ICollection<TblTaxaSynonyms> TblTaxaSynonyms { get; set; }
        public virtual ICollection<TblTaxaTreeMaster> TblTaxaTreeMaster { get; set; }
    }
}
