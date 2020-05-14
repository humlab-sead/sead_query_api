using System;
using System.Collections.Generic;

namespace SeadQueryTest.Infrastructure.Data.StudyModel.Model
{
    public partial class Cohort
    {
        public Cohort()
        {
            CohortDescription = new HashSet<CohortDescription>();
            Subject = new HashSet<Subject>();
        }

        public int CohortId { get; set; }
        public int? ProjectId { get; set; }
        public int MethodId { get; set; }
        public string Name { get; set; }

        public virtual Method Method { get; set; }
        public virtual Project Project { get; set; }
        public virtual ICollection<CohortDescription> CohortDescription { get; set; }
        public virtual ICollection<Subject> Subject { get; set; }
    }
}
