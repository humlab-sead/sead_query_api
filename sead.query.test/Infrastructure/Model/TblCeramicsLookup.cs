using System;
using System.Collections.Generic;

namespace SQT.Infrastructure.Model
{
    public partial class TblCeramicsLookup
    {
        public TblCeramicsLookup()
        {
            TblCeramics = new HashSet<TblCeramics>();
        }

        public int CeramicsLookupId { get; set; }
        public int MethodId { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public DateTime? DateUpdated { get; set; }

        public virtual TblMethods Method { get; set; }
        public virtual ICollection<TblCeramics> TblCeramics { get; set; }
    }
}
