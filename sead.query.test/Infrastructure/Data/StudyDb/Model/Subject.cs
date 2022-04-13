using System;
using System.Collections.Generic;

namespace SeadQueryTest.Infrastructure.Data.StudyModel.Model
{
    public partial class Subject
    {
        public Subject()
        {
            Experiment = new HashSet<Experiment>();
            SubjectNote = new HashSet<SubjectNote>();
        }

        public int SubjectId { get; set; }
        public int CohortId { get; set; }
        public int SubjectTypeId { get; set; }
        public string Name { get; set; }

        public virtual Cohort Cohort { get; set; }
        public virtual SubjectType SubjectType { get; set; }
        public virtual ICollection<Experiment> Experiment { get; set; }
        public virtual ICollection<SubjectNote> SubjectNote { get; set; }
    }
}
