using System;
using System.Collections.Generic;

namespace SQT.Infrastructure.Model
{
    public partial class TblSampleGroupNotes
    {
        public int SampleGroupNoteId { get; set; }
        public int SampleGroupId { get; set; }
        public string Note { get; set; }
        public DateTime? DateUpdated { get; set; }

        public virtual TblSampleGroups SampleGroup { get; set; }
    }
}
