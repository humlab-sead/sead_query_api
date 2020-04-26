using System;
using System.Collections.Generic;

namespace SQT.Infrastructure.Model
{
    public partial class TblRecordTypes
    {
        public TblRecordTypes()
        {
            TblAbundanceElements = new HashSet<TblAbundanceElements>();
            TblMethods = new HashSet<TblMethods>();
            TblSiteOtherRecords = new HashSet<TblSiteOtherRecords>();
            TblTaxaTreeOrders = new HashSet<TblTaxaTreeOrders>();
        }

        public int RecordTypeId { get; set; }
        public string RecordTypeName { get; set; }
        public string RecordTypeDescription { get; set; }
        public DateTime? DateUpdated { get; set; }

        public virtual ICollection<TblAbundanceElements> TblAbundanceElements { get; set; }
        public virtual ICollection<TblMethods> TblMethods { get; set; }
        public virtual ICollection<TblSiteOtherRecords> TblSiteOtherRecords { get; set; }
        public virtual ICollection<TblTaxaTreeOrders> TblTaxaTreeOrders { get; set; }
    }
}
