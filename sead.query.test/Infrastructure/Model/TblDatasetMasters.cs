using System;
using System.Collections.Generic;

namespace SQT.Infrastructure.Model
{
    public partial class TblDatasetMasters
    {
        public TblDatasetMasters()
        {
            TblDatasets = new HashSet<TblDatasets>();
        }

        public int MasterSetId { get; set; }
        public int? ContactId { get; set; }
        public int? BiblioId { get; set; }
        public string MasterName { get; set; }
        public string MasterNotes { get; set; }
        public string Url { get; set; }
        public DateTime? DateUpdated { get; set; }

        public virtual TblBiblio Biblio { get; set; }
        public virtual TblContacts Contact { get; set; }
        public virtual ICollection<TblDatasets> TblDatasets { get; set; }
    }
}
