using System;
using System.Collections.Generic;

namespace SQT.Infrastructure.Model
{
    public partial class TblSampleGroupDescriptionTypes
    {
        public TblSampleGroupDescriptionTypes()
        {
            TblSampleGroupDescriptionTypeSamplingContexts = new HashSet<TblSampleGroupDescriptionTypeSamplingContexts>();
            TblSampleGroupDescriptions = new HashSet<TblSampleGroupDescriptions>();
        }

        public int SampleGroupDescriptionTypeId { get; set; }
        public string TypeName { get; set; }
        public string TypeDescription { get; set; }
        public DateTime? DateUpdated { get; set; }

        public virtual ICollection<TblSampleGroupDescriptionTypeSamplingContexts> TblSampleGroupDescriptionTypeSamplingContexts { get; set; }
        public virtual ICollection<TblSampleGroupDescriptions> TblSampleGroupDescriptions { get; set; }
    }
}
