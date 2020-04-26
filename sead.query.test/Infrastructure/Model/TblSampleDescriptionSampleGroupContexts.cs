using System;
using System.Collections.Generic;

namespace SQT.Infrastructure.Model
{
    public partial class TblSampleDescriptionSampleGroupContexts
    {
        public int SampleDescriptionSampleGroupContextId { get; set; }
        public int? SamplingContextId { get; set; }
        public int? SampleDescriptionTypeId { get; set; }
        public DateTime? DateUpdated { get; set; }

        public virtual TblSampleDescriptionTypes SampleDescriptionType { get; set; }
        public virtual TblSampleGroupSamplingContexts SamplingContext { get; set; }
    }
}
