using System;
using System.Collections.Generic;

namespace SQT.Infrastructure.Model
{
    public partial class TblTaxaTreeFamilies
    {
        public TblTaxaTreeFamilies()
        {
            TblTaxaSynonyms = new HashSet<TblTaxaSynonyms>();
            TblTaxaTreeGenera = new HashSet<TblTaxaTreeGenera>();
        }

        public int FamilyId { get; set; }
        public DateTime? DateUpdated { get; set; }
        public string FamilyName { get; set; }
        public int OrderId { get; set; }

        public virtual TblTaxaTreeOrders Order { get; set; }
        public virtual ICollection<TblTaxaSynonyms> TblTaxaSynonyms { get; set; }
        public virtual ICollection<TblTaxaTreeGenera> TblTaxaTreeGenera { get; set; }
    }
}
