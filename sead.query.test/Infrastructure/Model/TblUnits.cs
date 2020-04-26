using System;
using System.Collections.Generic;

namespace SQT.Infrastructure.Model
{
    public partial class TblUnits
    {
        public TblUnits()
        {
            TblDimensions = new HashSet<TblDimensions>();
            TblIsotopes = new HashSet<TblIsotopes>();
            TblMethods = new HashSet<TblMethods>();
        }

        public int UnitId { get; set; }
        public DateTime? DateUpdated { get; set; }
        public string Description { get; set; }
        public string UnitAbbrev { get; set; }
        public string UnitName { get; set; }

        public virtual ICollection<TblDimensions> TblDimensions { get; set; }
        public virtual ICollection<TblIsotopes> TblIsotopes { get; set; }
        public virtual ICollection<TblMethods> TblMethods { get; set; }
    }
}
