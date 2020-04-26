using System;
using System.Collections.Generic;

namespace SQT.Infrastructure.Model
{
    public partial class TblAbundances
    {
        public TblAbundances()
        {
            TblAbundanceIdentLevels = new HashSet<TblAbundanceIdentLevels>();
            TblAbundanceModifications = new HashSet<TblAbundanceModifications>();
        }

        public int AbundanceId { get; set; }
        public int TaxonId { get; set; }
        public int AnalysisEntityId { get; set; }
        public int? AbundanceElementId { get; set; }
        public int Abundance { get; set; }
        public DateTime? DateUpdated { get; set; }

        public virtual TblAbundanceElements AbundanceElement { get; set; }
        public virtual TblAnalysisEntities AnalysisEntity { get; set; }
        public virtual TblTaxaTreeMaster Taxon { get; set; }
        public virtual ICollection<TblAbundanceIdentLevels> TblAbundanceIdentLevels { get; set; }
        public virtual ICollection<TblAbundanceModifications> TblAbundanceModifications { get; set; }
    }
}
