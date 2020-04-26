using System;
using System.Collections.Generic;

namespace SQT.Infrastructure.Model
{
    public partial class TblEcocodeSystems
    {
        public TblEcocodeSystems()
        {
            TblEcocodeGroups = new HashSet<TblEcocodeGroups>();
        }

        public int EcocodeSystemId { get; set; }
        public int? BiblioId { get; set; }
        public DateTime? DateUpdated { get; set; }
        public string Definition { get; set; }
        public string Name { get; set; }
        public string Notes { get; set; }

        public virtual TblBiblio Biblio { get; set; }
        public virtual ICollection<TblEcocodeGroups> TblEcocodeGroups { get; set; }
    }
}
