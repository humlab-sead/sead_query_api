using System;
using System.Collections.Generic;

namespace SQT.Infrastructure.Model
{
    public partial class TblSampleCoordinates
    {
        public int SampleCoordinateId { get; set; }
        public int PhysicalSampleId { get; set; }
        public int CoordinateMethodDimensionId { get; set; }
        public decimal Measurement { get; set; }
        public decimal? Accuracy { get; set; }
        public DateTime? DateUpdated { get; set; }

        public virtual TblCoordinateMethodDimensions CoordinateMethodDimension { get; set; }
        public virtual TblPhysicalSamples PhysicalSample { get; set; }
    }
}
