using System;
using System.Collections.Generic;

namespace SQT.Infrastructure.Model
{
    public partial class TblLithology
    {
        public int LithologyId { get; set; }
        public DateTime? DateUpdated { get; set; }
        public decimal? DepthBottom { get; set; }
        public decimal DepthTop { get; set; }
        public string Description { get; set; }
        public string LowerBoundary { get; set; }
        public int SampleGroupId { get; set; }

        public virtual TblSampleGroups SampleGroup { get; set; }
    }
}
