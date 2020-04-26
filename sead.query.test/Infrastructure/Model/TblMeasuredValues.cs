using System;
using System.Collections.Generic;

namespace SQT.Infrastructure.Model
{
    public partial class TblMeasuredValues
    {
        public TblMeasuredValues()
        {
            TblMeasuredValueDimensions = new HashSet<TblMeasuredValueDimensions>();
        }

        public int MeasuredValueId { get; set; }
        public int AnalysisEntityId { get; set; }
        public DateTime? DateUpdated { get; set; }
        public decimal MeasuredValue { get; set; }

        public virtual TblAnalysisEntities AnalysisEntity { get; set; }
        public virtual ICollection<TblMeasuredValueDimensions> TblMeasuredValueDimensions { get; set; }
    }
}
