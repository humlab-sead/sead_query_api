using System;
using System.Collections.Generic;

namespace SQT.Infrastructure.Model
{
    public partial class TblEcocodeGroups
    {
        public TblEcocodeGroups()
        {
            TblEcocodeDefinitions = new HashSet<TblEcocodeDefinitions>();
        }

        public int EcocodeGroupId { get; set; }
        public DateTime? DateUpdated { get; set; }
        public string Definition { get; set; }
        public int? EcocodeSystemId { get; set; }
        public string Name { get; set; }
        public string Abbreviation { get; set; }

        public virtual TblEcocodeSystems EcocodeSystem { get; set; }
        public virtual ICollection<TblEcocodeDefinitions> TblEcocodeDefinitions { get; set; }
    }
}
