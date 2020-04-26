using System;
using System.Collections.Generic;

namespace SQT.Infrastructure.Model
{
    public partial class TblActivityTypes
    {
        public TblActivityTypes()
        {
            TblTaxaSeasonality = new HashSet<TblTaxaSeasonality>();
        }

        public int ActivityTypeId { get; set; }
        public string ActivityType { get; set; }
        public string Description { get; set; }
        public DateTime? DateUpdated { get; set; }

        public virtual ICollection<TblTaxaSeasonality> TblTaxaSeasonality { get; set; }
    }
}
