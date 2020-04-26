using System;
using System.Collections.Generic;

namespace SQT.Infrastructure.Model
{
    public partial class TblTephraDates
    {
        public int TephraDateId { get; set; }
        public int AnalysisEntityId { get; set; }
        public DateTime? DateUpdated { get; set; }
        public string Notes { get; set; }
        public int TephraId { get; set; }
        public int? DatingUncertaintyId { get; set; }

        public virtual TblAnalysisEntities AnalysisEntity { get; set; }
        public virtual TblDatingUncertainty DatingUncertainty { get; set; }
        public virtual TblTephras Tephra { get; set; }
    }
}
