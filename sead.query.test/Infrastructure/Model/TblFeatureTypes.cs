using System;
using System.Collections.Generic;

namespace SQT.Infrastructure.Model
{
    public partial class TblFeatureTypes
    {
        public TblFeatureTypes()
        {
            TblFeatures = new HashSet<TblFeatures>();
        }

        public int FeatureTypeId { get; set; }
        public string FeatureTypeName { get; set; }
        public string FeatureTypeDescription { get; set; }
        public DateTime? DateUpdated { get; set; }

        public virtual ICollection<TblFeatures> TblFeatures { get; set; }
    }
}
