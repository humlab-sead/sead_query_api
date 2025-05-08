using System;
using System.Collections.Generic;

namespace SeadQueryTest.Infrastructure.Data.StudyModel.Model
{
    public partial class SubjectNote
    {
        public int SubjectNoteId { get; set; }
        public int SubjectId { get; set; }
        public string NoteType { get; set; }
        public string Note { get; set; }

        public virtual Subject Subject { get; set; }
    }
}
