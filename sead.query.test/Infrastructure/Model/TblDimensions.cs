using System;
using System.Collections.Generic;

namespace SQT.Infrastructure.Model
{
    public partial class TblDimensions
    {
        public TblDimensions()
        {
            TblAnalysisEntityDimensions = new HashSet<TblAnalysisEntityDimensions>();
            TblCoordinateMethodDimensions = new HashSet<TblCoordinateMethodDimensions>();
            TblMeasuredValueDimensions = new HashSet<TblMeasuredValueDimensions>();
            TblSampleDimensions = new HashSet<TblSampleDimensions>();
            TblSampleGroupDimensions = new HashSet<TblSampleGroupDimensions>();
        }

        public int DimensionId { get; set; }
        public DateTime? DateUpdated { get; set; }
        public string DimensionAbbrev { get; set; }
        public string DimensionDescription { get; set; }
        public string DimensionName { get; set; }
        public int? UnitId { get; set; }
        public int? MethodGroupId { get; set; }

        public virtual TblMethodGroups MethodGroup { get; set; }
        public virtual TblUnits Unit { get; set; }
        public virtual ICollection<TblAnalysisEntityDimensions> TblAnalysisEntityDimensions { get; set; }
        public virtual ICollection<TblCoordinateMethodDimensions> TblCoordinateMethodDimensions { get; set; }
        public virtual ICollection<TblMeasuredValueDimensions> TblMeasuredValueDimensions { get; set; }
        public virtual ICollection<TblSampleDimensions> TblSampleDimensions { get; set; }
        public virtual ICollection<TblSampleGroupDimensions> TblSampleGroupDimensions { get; set; }
    }
}
