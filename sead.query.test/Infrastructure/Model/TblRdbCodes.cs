using System;
using System.Collections.Generic;

namespace SQT.Infrastructure.Model
{
    public partial class TblRdbCodes
    {
        public TblRdbCodes()
        {
            TblRdb = new HashSet<TblRdb>();
        }

        public int RdbCodeId { get; set; }
        public DateTime? DateUpdated { get; set; }
        public string RdbCategory { get; set; }
        public string RdbDefinition { get; set; }
        public int? RdbSystemId { get; set; }

        public virtual TblRdbSystems RdbSystem { get; set; }
        public virtual ICollection<TblRdb> TblRdb { get; set; }
    }
}
