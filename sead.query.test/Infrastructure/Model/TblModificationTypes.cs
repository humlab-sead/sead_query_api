using System;
using System.Collections.Generic;

namespace SQT.Infrastructure.Model
{
    public partial class TblModificationTypes
    {
        public TblModificationTypes()
        {
            TblAbundanceModifications = new HashSet<TblAbundanceModifications>();
        }

        public int ModificationTypeId { get; set; }
        public string ModificationTypeName { get; set; }
        public string ModificationTypeDescription { get; set; }
        public DateTime? DateUpdated { get; set; }

        public virtual ICollection<TblAbundanceModifications> TblAbundanceModifications { get; set; }
    }
}
