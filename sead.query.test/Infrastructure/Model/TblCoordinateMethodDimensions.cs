using System;
using System.Collections.Generic;

namespace SQT.Infrastructure.Model
{
    public partial class TblCoordinateMethodDimensions
    {
        public TblCoordinateMethodDimensions()
        {
            TblSampleCoordinates = new HashSet<TblSampleCoordinates>();
            TblSampleGroupCoordinates = new HashSet<TblSampleGroupCoordinates>();
        }

        public int CoordinateMethodDimensionId { get; set; }
        public int DimensionId { get; set; }
        public int MethodId { get; set; }
        public decimal? LimitUpper { get; set; }
        public decimal? LimitLower { get; set; }
        public DateTime? DateUpdated { get; set; }

        public virtual TblDimensions Dimension { get; set; }
        public virtual TblMethods Method { get; set; }
        public virtual ICollection<TblSampleCoordinates> TblSampleCoordinates { get; set; }
        public virtual ICollection<TblSampleGroupCoordinates> TblSampleGroupCoordinates { get; set; }
    }
}
