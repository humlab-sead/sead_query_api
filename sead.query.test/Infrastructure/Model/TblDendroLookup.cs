using System;
using System.Collections.Generic;

namespace SQT.Infrastructure.Model
{
    public partial class TblDendroLookup
    {
        public TblDendroLookup()
        {
            TblDendro = new HashSet<TblDendro>();
            TblDendroDates = new HashSet<TblDendroDates>();
        }

        public int DendroLookupId { get; set; }
        public int? MethodId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime? DateUpdated { get; set; }

        public virtual TblMethods Method { get; set; }
        public virtual ICollection<TblDendro> TblDendro { get; set; }
        public virtual ICollection<TblDendroDates> TblDendroDates { get; set; }
    }
}
