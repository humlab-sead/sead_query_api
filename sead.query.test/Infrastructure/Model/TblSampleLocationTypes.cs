using System;
using System.Collections.Generic;

namespace SQT.Infrastructure.Model
{
    public partial class TblSampleLocationTypes
    {
        public TblSampleLocationTypes()
        {
            TblSampleLocationTypeSamplingContexts = new HashSet<TblSampleLocationTypeSamplingContexts>();
            TblSampleLocations = new HashSet<TblSampleLocations>();
        }

        public int SampleLocationTypeId { get; set; }
        public string LocationType { get; set; }
        public string LocationTypeDescription { get; set; }
        public DateTime? DateUpdated { get; set; }

        public virtual ICollection<TblSampleLocationTypeSamplingContexts> TblSampleLocationTypeSamplingContexts { get; set; }
        public virtual ICollection<TblSampleLocations> TblSampleLocations { get; set; }
    }
}
