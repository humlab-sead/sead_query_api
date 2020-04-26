using System;
using System.Collections.Generic;

namespace SQT.Infrastructure.Model
{
    public partial class TblSampleGroupDescriptionTypeSamplingContexts
    {
        public int SampleGroupDescriptionTypeSamplingContextId { get; set; }
        public int SamplingContextId { get; set; }
        public int SampleGroupDescriptionTypeId { get; set; }
        public DateTime? DateUpdated { get; set; }

        public virtual TblSampleGroupDescriptionTypes SampleGroupDescriptionType { get; set; }
        public virtual TblSampleGroupSamplingContexts SamplingContext { get; set; }
    }
}
