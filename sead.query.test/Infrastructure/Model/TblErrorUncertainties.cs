using System;
using System.Collections.Generic;

namespace SQT.Infrastructure.Model
{
    public partial class TblErrorUncertainties
    {
        public TblErrorUncertainties()
        {
            TblDendroDates = new HashSet<TblDendroDates>();
        }

        public int ErrorUncertaintyId { get; set; }
        public string ErrorUncertaintyType { get; set; }
        public string Description { get; set; }
        public DateTime? DateUpdated { get; set; }

        public virtual ICollection<TblDendroDates> TblDendroDates { get; set; }
    }
}
