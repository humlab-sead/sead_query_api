using System;
using System.Collections.Generic;

namespace SQT.Infrastructure.Model
{
    public partial class TblGeochronRefs
    {
        public int GeochronRefId { get; set; }
        public int GeochronId { get; set; }
        public int BiblioId { get; set; }
        public DateTime? DateUpdated { get; set; }

        public virtual TblBiblio Biblio { get; set; }
        public virtual TblGeochronology Geochron { get; set; }
    }
}
