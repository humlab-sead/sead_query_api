using System;
using System.Collections.Generic;

namespace SQT.Infrastructure.Model
{
    public partial class TblSampleDimensions
    {
        public int SampleDimensionId { get; set; }
        public int PhysicalSampleId { get; set; }
        public int DimensionId { get; set; }
        public int MethodId { get; set; }
        public decimal DimensionValue { get; set; }
        public DateTime? DateUpdated { get; set; }

        public virtual TblDimensions Dimension { get; set; }
        public virtual TblMethods Method { get; set; }
        public virtual TblPhysicalSamples PhysicalSample { get; set; }
    }
}
