using System;
using System.Collections.Generic;

namespace SQT.Infrastructure.Model
{
    public partial class TblSeasons
    {
        public TblSeasons()
        {
            TblTaxaSeasonality = new HashSet<TblTaxaSeasonality>();
        }

        public int SeasonId { get; set; }
        public DateTime? DateUpdated { get; set; }
        public string SeasonName { get; set; }
        public string SeasonType { get; set; }
        public int? SeasonTypeId { get; set; }
        public short? SortOrder { get; set; }

        public virtual TblSeasonTypes SeasonTypeNavigation { get; set; }
        public virtual ICollection<TblTaxaSeasonality> TblTaxaSeasonality { get; set; }
    }
}
