using System;
using System.Collections.Generic;

namespace SQT.Infrastructure.Model
{
    public partial class TblSeasonTypes
    {
        public TblSeasonTypes()
        {
            TblSeasons = new HashSet<TblSeasons>();
        }

        public int SeasonTypeId { get; set; }
        public DateTime? DateUpdated { get; set; }
        public string Description { get; set; }
        public string SeasonType { get; set; }

        public virtual ICollection<TblSeasons> TblSeasons { get; set; }
    }
}
