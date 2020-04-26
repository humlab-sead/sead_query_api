using System;
using System.Collections.Generic;

namespace SQT.Infrastructure.Model
{
    public partial class TblTaxaSeasonality
    {
        public int SeasonalityId { get; set; }
        public int ActivityTypeId { get; set; }
        public int? SeasonId { get; set; }
        public int TaxonId { get; set; }
        public int LocationId { get; set; }
        public DateTime? DateUpdated { get; set; }

        public virtual TblActivityTypes ActivityType { get; set; }
        public virtual TblLocations Location { get; set; }
        public virtual TblSeasons Season { get; set; }
        public virtual TblTaxaTreeMaster Taxon { get; set; }
    }
}
