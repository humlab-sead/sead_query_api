using System;
using System.Collections.Generic;

namespace SQT.Infrastructure.Model
{
    public partial class TblMeasuredValueDimensions
    {
        public int MeasuredValueDimensionId { get; set; }
        public DateTime? DateUpdated { get; set; }
        public int DimensionId { get; set; }
        public decimal DimensionValue { get; set; }
        public int MeasuredValueId { get; set; }

        public virtual TblDimensions Dimension { get; set; }
        public virtual TblMeasuredValues MeasuredValue { get; set; }
    }
}
