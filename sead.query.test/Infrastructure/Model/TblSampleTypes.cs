using System;
using System.Collections.Generic;

namespace SQT.Infrastructure.Model
{
    public partial class TblSampleTypes
    {
        public TblSampleTypes()
        {
            TblPhysicalSamples = new HashSet<TblPhysicalSamples>();
        }

        public int SampleTypeId { get; set; }
        public string TypeName { get; set; }
        public string Description { get; set; }
        public DateTime? DateUpdated { get; set; }

        public virtual ICollection<TblPhysicalSamples> TblPhysicalSamples { get; set; }
    }
}
