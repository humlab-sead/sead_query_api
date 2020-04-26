using System;
using System.Collections.Generic;

namespace SQT.Infrastructure.Model
{
    public partial class TblSampleAltRefs
    {
        public int SampleAltRefId { get; set; }
        public string AltRef { get; set; }
        public int AltRefTypeId { get; set; }
        public DateTime? DateUpdated { get; set; }
        public int PhysicalSampleId { get; set; }

        public virtual TblAltRefTypes AltRefType { get; set; }
        public virtual TblPhysicalSamples PhysicalSample { get; set; }
    }
}
