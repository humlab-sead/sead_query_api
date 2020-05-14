using System;
using System.Collections.Generic;

namespace SeadQueryTest.Infrastructure.Data.StudyModel.Model
{
    public partial class CohortDescription
    {
        public int CohortDescriptionId { get; set; }
        public string Description { get; set; }
        public int CohortId { get; set; }

        public virtual Cohort Cohort { get; set; }
    }
}
