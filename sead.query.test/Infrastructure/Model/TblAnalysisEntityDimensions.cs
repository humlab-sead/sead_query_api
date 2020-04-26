using System;
using System.Collections.Generic;

namespace SQT.Infrastructure.Model
{
    public partial class TblAnalysisEntityDimensions
    {
        public int AnalysisEntityDimensionId { get; set; }
        public int AnalysisEntityId { get; set; }
        public int DimensionId { get; set; }
        public decimal DimensionValue { get; set; }
        public DateTime? DateUpdated { get; set; }

        public virtual TblAnalysisEntities AnalysisEntity { get; set; }
        public virtual TblDimensions Dimension { get; set; }
    }
}
