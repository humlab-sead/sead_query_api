using System;
using System.Collections.Generic;

namespace SQT.Infrastructure.Model
{
    public partial class TblFeatures
    {
        public TblFeatures()
        {
            TblPhysicalSampleFeatures = new HashSet<TblPhysicalSampleFeatures>();
        }

        public int FeatureId { get; set; }
        public int FeatureTypeId { get; set; }
        public string FeatureName { get; set; }
        public string FeatureDescription { get; set; }
        public DateTime? DateUpdated { get; set; }

        public virtual TblFeatureTypes FeatureType { get; set; }
        public virtual ICollection<TblPhysicalSampleFeatures> TblPhysicalSampleFeatures { get; set; }
    }
}
