using System;
using System.Collections.Generic;

namespace SQT.Infrastructure.Model
{
    public partial class TblSampleGroupSamplingContexts
    {
        public TblSampleGroupSamplingContexts()
        {
            TblSampleDescriptionSampleGroupContexts = new HashSet<TblSampleDescriptionSampleGroupContexts>();
            TblSampleGroupDescriptionTypeSamplingContexts = new HashSet<TblSampleGroupDescriptionTypeSamplingContexts>();
            TblSampleGroups = new HashSet<TblSampleGroups>();
            TblSampleLocationTypeSamplingContexts = new HashSet<TblSampleLocationTypeSamplingContexts>();
        }

        public int SamplingContextId { get; set; }
        public string SamplingContext { get; set; }
        public string Description { get; set; }
        public short SortOrder { get; set; }
        public DateTime? DateUpdated { get; set; }

        public virtual ICollection<TblSampleDescriptionSampleGroupContexts> TblSampleDescriptionSampleGroupContexts { get; set; }
        public virtual ICollection<TblSampleGroupDescriptionTypeSamplingContexts> TblSampleGroupDescriptionTypeSamplingContexts { get; set; }
        public virtual ICollection<TblSampleGroups> TblSampleGroups { get; set; }
        public virtual ICollection<TblSampleLocationTypeSamplingContexts> TblSampleLocationTypeSamplingContexts { get; set; }
    }
}
