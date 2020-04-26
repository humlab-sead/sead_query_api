using System;
using System.Collections.Generic;

namespace SQT.Infrastructure.Model
{
    public partial class TblAltRefTypes
    {
        public TblAltRefTypes()
        {
            TblPhysicalSamples = new HashSet<TblPhysicalSamples>();
            TblSampleAltRefs = new HashSet<TblSampleAltRefs>();
        }

        public int AltRefTypeId { get; set; }
        public string AltRefType { get; set; }
        public DateTime? DateUpdated { get; set; }
        public string Description { get; set; }

        public virtual ICollection<TblPhysicalSamples> TblPhysicalSamples { get; set; }
        public virtual ICollection<TblSampleAltRefs> TblSampleAltRefs { get; set; }
    }
}
