using System;
using System.Collections.Generic;

namespace SeadQueryTest.Infrastructure.Data.StudyModel.Model
{
    public partial class Study
    {
        public Study()
        {
            Experiment = new HashSet<Experiment>();
        }

        public int StudyId { get; set; }
        public int StudyTypeId { get; set; }
        public int MethodId { get; set; }
        public int ReportId { get; set; }
        public string StudyName { get; set; }

        public virtual Method Method { get; set; }
        public virtual Report Report { get; set; }
        public virtual StudyType StudyType { get; set; }
        public virtual ICollection<Experiment> Experiment { get; set; }
    }
}
