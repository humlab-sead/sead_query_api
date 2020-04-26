using System;
using System.Collections.Generic;

namespace SQT.Infrastructure.Model
{
    public partial class TblTaxaTreeOrders
    {
        public TblTaxaTreeOrders()
        {
            TblTaxaTreeFamilies = new HashSet<TblTaxaTreeFamilies>();
        }

        public int OrderId { get; set; }
        public DateTime? DateUpdated { get; set; }
        public string OrderName { get; set; }
        public int? RecordTypeId { get; set; }
        public int? SortOrder { get; set; }

        public virtual TblRecordTypes RecordType { get; set; }
        public virtual ICollection<TblTaxaTreeFamilies> TblTaxaTreeFamilies { get; set; }
    }
}
