using System;
using System.Collections.Generic;

namespace SQT.Infrastructure.Model
{
    public partial class TblAgeTypes
    {
        public TblAgeTypes()
        {
            TblDendroDates = new HashSet<TblDendroDates>();
        }

        public int AgeTypeId { get; set; }
        public string AgeType { get; set; }
        public string Description { get; set; }
        public DateTime? DateUpdated { get; set; }

        public virtual ICollection<TblDendroDates> TblDendroDates { get; set; }
    }
}
