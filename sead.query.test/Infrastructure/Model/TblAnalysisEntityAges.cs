using System;
using System.Collections.Generic;

namespace SQT.Infrastructure.Model
{
    public partial class TblAnalysisEntityAges
    {
        public TblAnalysisEntityAges()
        {
            TblAggregateSampleAges = new HashSet<TblAggregateSampleAges>();
        }

        public int AnalysisEntityAgeId { get; set; }
        public decimal? Age { get; set; }
        public decimal? AgeOlder { get; set; }
        public decimal? AgeYounger { get; set; }
        public int? AnalysisEntityId { get; set; }
        public int? ChronologyId { get; set; }
        public DateTime? DateUpdated { get; set; }

        public virtual TblAnalysisEntities AnalysisEntity { get; set; }
        public virtual TblChronologies Chronology { get; set; }
        public virtual ICollection<TblAggregateSampleAges> TblAggregateSampleAges { get; set; }
    }
}
