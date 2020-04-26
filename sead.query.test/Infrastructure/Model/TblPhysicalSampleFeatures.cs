using System;
using System.Collections.Generic;

namespace SQT.Infrastructure.Model
{
    public partial class TblPhysicalSampleFeatures
    {
        public int PhysicalSampleFeatureId { get; set; }
        public DateTime? DateUpdated { get; set; }
        public int FeatureId { get; set; }
        public int PhysicalSampleId { get; set; }

        public virtual TblFeatures Feature { get; set; }
        public virtual TblPhysicalSamples PhysicalSample { get; set; }
    }
}
