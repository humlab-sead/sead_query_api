using System;
using System.Collections.Generic;

namespace SQT.Infrastructure.Model
{
    public partial class TblSampleLocationTypeSamplingContexts
    {
        public int SampleLocationTypeSamplingContextId { get; set; }
        public int SamplingContextId { get; set; }
        public int SampleLocationTypeId { get; set; }
        public DateTime? DateUpdated { get; set; }

        public virtual TblSampleLocationTypes SampleLocationType { get; set; }
        public virtual TblSampleGroupSamplingContexts SamplingContext { get; set; }
    }
}
