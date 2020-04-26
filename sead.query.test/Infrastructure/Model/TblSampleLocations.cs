using System;
using System.Collections.Generic;

namespace SQT.Infrastructure.Model
{
    public partial class TblSampleLocations
    {
        public int SampleLocationId { get; set; }
        public int SampleLocationTypeId { get; set; }
        public int PhysicalSampleId { get; set; }
        public string Location { get; set; }
        public DateTime? DateUpdated { get; set; }

        public virtual TblPhysicalSamples PhysicalSample { get; set; }
        public virtual TblSampleLocationTypes SampleLocationType { get; set; }
    }
}
