using System;
using System.Collections.Generic;

namespace SQT.Infrastructure.Model
{
    public partial class TblSampleNotes
    {
        public int SampleNoteId { get; set; }
        public int PhysicalSampleId { get; set; }
        public string NoteType { get; set; }
        public string Note { get; set; }
        public DateTime? DateUpdated { get; set; }

        public virtual TblPhysicalSamples PhysicalSample { get; set; }
    }
}
