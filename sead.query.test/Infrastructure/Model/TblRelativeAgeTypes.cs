using System;
using System.Collections.Generic;

namespace SQT.Infrastructure.Model
{
    public partial class TblRelativeAgeTypes
    {
        public TblRelativeAgeTypes()
        {
            TblRelativeAges = new HashSet<TblRelativeAges>();
        }

        public int RelativeAgeTypeId { get; set; }
        public string AgeType { get; set; }
        public string Description { get; set; }
        public DateTime? DateUpdated { get; set; }

        public virtual ICollection<TblRelativeAges> TblRelativeAges { get; set; }
    }
}
