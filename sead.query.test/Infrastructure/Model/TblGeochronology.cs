using System;
using System.Collections.Generic;

namespace SQT.Infrastructure.Model
{
    public partial class TblGeochronology
    {
        public TblGeochronology()
        {
            TblDatingMaterial = new HashSet<TblDatingMaterial>();
            TblGeochronRefs = new HashSet<TblGeochronRefs>();
        }

        public int GeochronId { get; set; }
        public int AnalysisEntityId { get; set; }
        public int? DatingLabId { get; set; }
        public string LabNumber { get; set; }
        public decimal? Age { get; set; }
        public decimal? ErrorOlder { get; set; }
        public decimal? ErrorYounger { get; set; }
        public decimal? Delta13c { get; set; }
        public string Notes { get; set; }
        public DateTime? DateUpdated { get; set; }
        public int? DatingUncertaintyId { get; set; }

        public virtual TblAnalysisEntities AnalysisEntity { get; set; }
        public virtual TblDatingLabs DatingLab { get; set; }
        public virtual TblDatingUncertainty DatingUncertainty { get; set; }
        public virtual ICollection<TblDatingMaterial> TblDatingMaterial { get; set; }
        public virtual ICollection<TblGeochronRefs> TblGeochronRefs { get; set; }
    }
}
