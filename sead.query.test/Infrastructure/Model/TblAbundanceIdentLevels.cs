using System;
using System.Collections.Generic;

namespace SQT.Infrastructure.Model
{
    public partial class TblAbundanceIdentLevels
    {
        public int AbundanceIdentLevelId { get; set; }
        public int AbundanceId { get; set; }
        public int IdentificationLevelId { get; set; }
        public DateTime? DateUpdated { get; set; }

        public virtual TblAbundances Abundance { get; set; }
        public virtual TblIdentificationLevels IdentificationLevel { get; set; }
    }
}
