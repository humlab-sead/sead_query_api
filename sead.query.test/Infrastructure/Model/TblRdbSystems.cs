using System;
using System.Collections.Generic;

namespace SQT.Infrastructure.Model
{
    public partial class TblRdbSystems
    {
        public TblRdbSystems()
        {
            TblRdbCodes = new HashSet<TblRdbCodes>();
        }

        public int RdbSystemId { get; set; }
        public int BiblioId { get; set; }
        public int LocationId { get; set; }
        public short? RdbFirstPublished { get; set; }
        public string RdbSystem { get; set; }
        public int? RdbSystemDate { get; set; }
        public string RdbVersion { get; set; }
        public DateTime? DateUpdated { get; set; }

        public virtual TblBiblio Biblio { get; set; }
        public virtual TblLocations Location { get; set; }
        public virtual ICollection<TblRdbCodes> TblRdbCodes { get; set; }
    }
}
