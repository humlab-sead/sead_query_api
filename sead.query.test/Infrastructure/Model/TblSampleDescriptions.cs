using System;
using System.Collections.Generic;

namespace SQT.Infrastructure.Model
{
    public partial class TblSampleDescriptions
    {
        public int SampleDescriptionId { get; set; }
        public int SampleDescriptionTypeId { get; set; }
        public int PhysicalSampleId { get; set; }
        public string Description { get; set; }
        public DateTime? DateUpdated { get; set; }

        public virtual TblPhysicalSamples PhysicalSample { get; set; }
        public virtual TblSampleDescriptionTypes SampleDescriptionType { get; set; }
    }
}
