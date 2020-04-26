using System;
using System.Collections.Generic;

namespace SQT.Infrastructure.Model
{
    public partial class TblTephraRefs
    {
        public int TephraRefId { get; set; }
        public int BiblioId { get; set; }
        public DateTime? DateUpdated { get; set; }
        public int TephraId { get; set; }

        public virtual TblBiblio Biblio { get; set; }
        public virtual TblTephras Tephra { get; set; }
    }
}
