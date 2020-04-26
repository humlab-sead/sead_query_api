using System;
using System.Collections.Generic;

namespace SQT.Infrastructure.Model
{
    public partial class TblSampleGroupReferences
    {
        public int SampleGroupReferenceId { get; set; }
        public int BiblioId { get; set; }
        public DateTime? DateUpdated { get; set; }
        public int? SampleGroupId { get; set; }

        public virtual TblBiblio Biblio { get; set; }
        public virtual TblSampleGroups SampleGroup { get; set; }
    }
}
