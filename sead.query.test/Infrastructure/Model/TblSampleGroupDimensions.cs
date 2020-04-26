using System;
using System.Collections.Generic;

namespace SQT.Infrastructure.Model
{
    public partial class TblSampleGroupDimensions
    {
        public int SampleGroupDimensionId { get; set; }
        public DateTime? DateUpdated { get; set; }
        public int DimensionId { get; set; }
        public decimal DimensionValue { get; set; }
        public int SampleGroupId { get; set; }

        public virtual TblDimensions Dimension { get; set; }
        public virtual TblSampleGroups SampleGroup { get; set; }
    }
}
