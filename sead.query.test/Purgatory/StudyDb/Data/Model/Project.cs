using System;
using System.Collections.Generic;

namespace SeadQueryTest.Infrastructure.Data.StudyModel.Model
{
    public partial class Project
    {
        public Project()
        {
            Cohort = new HashSet<Cohort>();
            ProjectProperty = new HashSet<ProjectProperty>();
            ProjectReport = new HashSet<ProjectReport>();
            ProjectResidence = new HashSet<ProjectResidence>();
        }

        public int ProjectId { get; set; }
        public int ProjectTypeId { get; set; }
        public decimal LatitudeDd { get; set; }
        public decimal LongitudeDd { get; set; }
        public string Name { get; set; }

        public virtual ProjectType ProjectType { get; set; }
        public virtual ICollection<Cohort> Cohort { get; set; }
        public virtual ICollection<ProjectProperty> ProjectProperty { get; set; }
        public virtual ICollection<ProjectReport> ProjectReport { get; set; }
        public virtual ICollection<ProjectResidence> ProjectResidence { get; set; }
    }
}
