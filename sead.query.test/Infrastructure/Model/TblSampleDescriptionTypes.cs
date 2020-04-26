using System;
using System.Collections.Generic;

namespace SQT.Infrastructure.Model
{
    public partial class TblSampleDescriptionTypes
    {
        public TblSampleDescriptionTypes()
        {
            TblSampleDescriptionSampleGroupContexts = new HashSet<TblSampleDescriptionSampleGroupContexts>();
            TblSampleDescriptions = new HashSet<TblSampleDescriptions>();
        }

        public int SampleDescriptionTypeId { get; set; }
        public string TypeName { get; set; }
        public string TypeDescription { get; set; }
        public DateTime? DateUpdated { get; set; }

        public virtual ICollection<TblSampleDescriptionSampleGroupContexts> TblSampleDescriptionSampleGroupContexts { get; set; }
        public virtual ICollection<TblSampleDescriptions> TblSampleDescriptions { get; set; }
    }
}
