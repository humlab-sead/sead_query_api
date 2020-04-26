using System;
using System.Collections.Generic;

namespace SQT.Infrastructure.Model
{
    public partial class TblAbundanceModifications
    {
        public int AbundanceModificationId { get; set; }
        public int AbundanceId { get; set; }
        public int ModificationTypeId { get; set; }
        public DateTime? DateUpdated { get; set; }

        public virtual TblAbundances Abundance { get; set; }
        public virtual TblModificationTypes ModificationType { get; set; }
    }
}
