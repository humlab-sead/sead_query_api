using System;
using System.Collections.Generic;

namespace SQT.Infrastructure.Model
{
    public partial class TblLocationTypes
    {
        public TblLocationTypes()
        {
            TblLocations = new HashSet<TblLocations>();
        }

        public int LocationTypeId { get; set; }
        public DateTime? DateUpdated { get; set; }
        public string Description { get; set; }
        public string LocationType { get; set; }

        public virtual ICollection<TblLocations> TblLocations { get; set; }
    }
}
