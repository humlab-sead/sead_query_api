using System;
using System.Collections.Generic;

namespace SQT.Infrastructure.Model
{
    public partial class TblAnalysisEntityPrepMethods
    {
        public int AnalysisEntityPrepMethodId { get; set; }
        public int AnalysisEntityId { get; set; }
        public int MethodId { get; set; }
        public DateTime? DateUpdated { get; set; }

        public virtual TblAnalysisEntities AnalysisEntity { get; set; }
        public virtual TblMethods Method { get; set; }
    }
}
