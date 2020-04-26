using System;
using System.Collections.Generic;

namespace SQT.Infrastructure.Model
{
    public partial class TblSampleGroupCoordinates
    {
        public int SampleGroupPositionId { get; set; }
        public int CoordinateMethodDimensionId { get; set; }
        public decimal? SampleGroupPosition { get; set; }
        public string PositionAccuracy { get; set; }
        public int SampleGroupId { get; set; }
        public DateTime? DateUpdated { get; set; }

        public virtual TblCoordinateMethodDimensions CoordinateMethodDimension { get; set; }
        public virtual TblSampleGroups SampleGroup { get; set; }
    }
}
