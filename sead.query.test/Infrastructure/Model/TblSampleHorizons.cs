using System;
using System.Collections.Generic;

namespace SQT.Infrastructure.Model
{
    public partial class TblSampleHorizons
    {
        public int SampleHorizonId { get; set; }
        public DateTime? DateUpdated { get; set; }
        public int HorizonId { get; set; }
        public int PhysicalSampleId { get; set; }

        public virtual TblHorizons Horizon { get; set; }
        public virtual TblPhysicalSamples PhysicalSample { get; set; }
    }
}
