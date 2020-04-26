using System;
using System.Collections.Generic;

namespace SQT.Infrastructure.Model
{
    public partial class TblAbundanceElements
    {
        public TblAbundanceElements()
        {
            TblAbundances = new HashSet<TblAbundances>();
            TblDatingMaterial = new HashSet<TblDatingMaterial>();
        }

        public int AbundanceElementId { get; set; }
        public int? RecordTypeId { get; set; }
        public string ElementName { get; set; }
        public string ElementDescription { get; set; }
        public DateTime? DateUpdated { get; set; }

        public virtual TblRecordTypes RecordType { get; set; }
        public virtual ICollection<TblAbundances> TblAbundances { get; set; }
        public virtual ICollection<TblDatingMaterial> TblDatingMaterial { get; set; }
    }
}
