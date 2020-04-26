using System;
using System.Collections.Generic;

namespace SQT.Infrastructure.Model
{
    public partial class TblSampleColours
    {
        public int SampleColourId { get; set; }
        public int ColourId { get; set; }
        public DateTime? DateUpdated { get; set; }
        public int PhysicalSampleId { get; set; }

        public virtual TblColours Colour { get; set; }
        public virtual TblPhysicalSamples PhysicalSample { get; set; }
    }
}
