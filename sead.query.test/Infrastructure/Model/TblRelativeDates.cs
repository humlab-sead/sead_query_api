using System;
using System.Collections.Generic;

namespace SQT.Infrastructure.Model
{
    public partial class TblRelativeDates
    {
        public int RelativeDateId { get; set; }
        public int? RelativeAgeId { get; set; }
        public int? MethodId { get; set; }
        public string Notes { get; set; }
        public DateTime? DateUpdated { get; set; }
        public int? DatingUncertaintyId { get; set; }
        public int AnalysisEntityId { get; set; }

        public virtual TblAnalysisEntities AnalysisEntity { get; set; }
        public virtual TblDatingUncertainty DatingUncertainty { get; set; }
        public virtual TblMethods Method { get; set; }
        public virtual TblRelativeAges RelativeAge { get; set; }
    }
}
