using System;
using System.Collections.Generic;

namespace SQT.Infrastructure.Model
{
    public partial class TblDatingMaterial
    {
        public int DatingMaterialId { get; set; }
        public int GeochronId { get; set; }
        public int? TaxonId { get; set; }
        public string MaterialDated { get; set; }
        public string Description { get; set; }
        public int? AbundanceElementId { get; set; }
        public DateTime? DateUpdated { get; set; }

        public virtual TblAbundanceElements AbundanceElement { get; set; }
        public virtual TblGeochronology Geochron { get; set; }
        public virtual TblTaxaTreeMaster Taxon { get; set; }
    }
}
