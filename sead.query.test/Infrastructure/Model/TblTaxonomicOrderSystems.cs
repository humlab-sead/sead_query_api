using System;
using System.Collections.Generic;

namespace SQT.Infrastructure.Model
{
    public partial class TblTaxonomicOrderSystems
    {
        public TblTaxonomicOrderSystems()
        {
            TblTaxonomicOrder = new HashSet<TblTaxonomicOrder>();
            TblTaxonomicOrderBiblio = new HashSet<TblTaxonomicOrderBiblio>();
        }

        public int TaxonomicOrderSystemId { get; set; }
        public DateTime? DateUpdated { get; set; }
        public string SystemDescription { get; set; }
        public string SystemName { get; set; }

        public virtual ICollection<TblTaxonomicOrder> TblTaxonomicOrder { get; set; }
        public virtual ICollection<TblTaxonomicOrderBiblio> TblTaxonomicOrderBiblio { get; set; }
    }
}
