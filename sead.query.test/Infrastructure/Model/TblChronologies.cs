using System;
using System.Collections.Generic;

namespace SQT.Infrastructure.Model
{
    public partial class TblChronologies
    {
        public TblChronologies()
        {
            TblAnalysisEntityAges = new HashSet<TblAnalysisEntityAges>();
            TblChronControls = new HashSet<TblChronControls>();
        }

        public int ChronologyId { get; set; }
        public int? AgeBoundOlder { get; set; }
        public int? AgeBoundYounger { get; set; }
        public string AgeModel { get; set; }
        public string ChronologyName { get; set; }
        public int? ContactId { get; set; }
        public DateTime? DatePrepared { get; set; }
        public DateTime? DateUpdated { get; set; }
        public bool IsDefault { get; set; }
        public string Notes { get; set; }
        public int? SampleGroupId { get; set; }
        public int? RelativeAgeTypeId { get; set; }

        public virtual TblContacts Contact { get; set; }
        public virtual TblSampleGroups SampleGroup { get; set; }
        public virtual ICollection<TblAnalysisEntityAges> TblAnalysisEntityAges { get; set; }
        public virtual ICollection<TblChronControls> TblChronControls { get; set; }
    }
}
