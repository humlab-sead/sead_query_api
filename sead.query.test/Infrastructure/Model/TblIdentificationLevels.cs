using System;
using System.Collections.Generic;

namespace SQT.Infrastructure.Model
{
    public partial class TblIdentificationLevels
    {
        public TblIdentificationLevels()
        {
            TblAbundanceIdentLevels = new HashSet<TblAbundanceIdentLevels>();
        }

        public int IdentificationLevelId { get; set; }
        public string IdentificationLevelAbbrev { get; set; }
        public string IdentificationLevelName { get; set; }
        public string Notes { get; set; }
        public DateTime? DateUpdated { get; set; }

        public virtual ICollection<TblAbundanceIdentLevels> TblAbundanceIdentLevels { get; set; }
    }
}
