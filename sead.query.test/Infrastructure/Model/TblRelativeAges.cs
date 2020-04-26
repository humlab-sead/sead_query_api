using System;
using System.Collections.Generic;

namespace SQT.Infrastructure.Model
{
    public partial class TblRelativeAges
    {
        public TblRelativeAges()
        {
            TblRelativeAgeRefs = new HashSet<TblRelativeAgeRefs>();
            TblRelativeDates = new HashSet<TblRelativeDates>();
        }

        public int RelativeAgeId { get; set; }
        public int? RelativeAgeTypeId { get; set; }
        public string RelativeAgeName { get; set; }
        public string Description { get; set; }
        public decimal? C14AgeOlder { get; set; }
        public decimal? C14AgeYounger { get; set; }
        public decimal? CalAgeOlder { get; set; }
        public decimal? CalAgeYounger { get; set; }
        public string Notes { get; set; }
        public DateTime? DateUpdated { get; set; }
        public int? LocationId { get; set; }
        public string Abbreviation { get; set; }

        public virtual TblLocations Location { get; set; }
        public virtual TblRelativeAgeTypes RelativeAgeType { get; set; }
        public virtual ICollection<TblRelativeAgeRefs> TblRelativeAgeRefs { get; set; }
        public virtual ICollection<TblRelativeDates> TblRelativeDates { get; set; }
    }
}
