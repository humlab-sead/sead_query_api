using System;
using System.Collections.Generic;

namespace SQT.Infrastructure.Model
{
    public partial class TblDendroDateNotes
    {
        public int DendroDateNoteId { get; set; }
        public int DendroDateId { get; set; }
        public string Note { get; set; }
        public DateTime? DateUpdated { get; set; }

        public virtual TblDendroDates DendroDate { get; set; }
    }
}
