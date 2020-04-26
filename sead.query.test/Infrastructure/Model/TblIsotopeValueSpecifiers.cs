using System;
using System.Collections.Generic;

namespace SQT.Infrastructure.Model
{
    public partial class TblIsotopeValueSpecifiers
    {
        public TblIsotopeValueSpecifiers()
        {
            TblIsotopes = new HashSet<TblIsotopes>();
        }

        public int IsotopeValueSpecifierId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime? DateUpdated { get; set; }

        public virtual ICollection<TblIsotopes> TblIsotopes { get; set; }
    }
}
