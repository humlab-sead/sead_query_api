using System;
using System.Collections.Generic;

namespace SQT.Infrastructure.Model
{
    public partial class TblHorizons
    {
        public TblHorizons()
        {
            TblSampleHorizons = new HashSet<TblSampleHorizons>();
        }

        public int HorizonId { get; set; }
        public DateTime? DateUpdated { get; set; }
        public string Description { get; set; }
        public string HorizonName { get; set; }
        public int MethodId { get; set; }

        public virtual TblMethods Method { get; set; }
        public virtual ICollection<TblSampleHorizons> TblSampleHorizons { get; set; }
    }
}
