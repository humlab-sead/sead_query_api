using System;
using System.Collections.Generic;

namespace SQT.Infrastructure.Model
{
    public partial class TblChronControlTypes
    {
        public TblChronControlTypes()
        {
            TblChronControls = new HashSet<TblChronControls>();
        }

        public int ChronControlTypeId { get; set; }
        public string ChronControlType { get; set; }
        public DateTime? DateUpdated { get; set; }

        public virtual ICollection<TblChronControls> TblChronControls { get; set; }
    }
}
