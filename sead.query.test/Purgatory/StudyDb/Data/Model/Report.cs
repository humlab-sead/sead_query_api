using System;
using System.Collections.Generic;

namespace SeadQueryTest.Infrastructure.Data.StudyModel.Model
{
    public partial class Report
    {
        public Report()
        {
            Method = new HashSet<Method>();
            ProjectReport = new HashSet<ProjectReport>();
            Study = new HashSet<Study>();
        }

        public int ReportId { get; set; }
        public string Title { get; set; }
        public string Year { get; set; }
        public string Author { get; set; }

        public virtual ICollection<Method> Method { get; set; }
        public virtual ICollection<ProjectReport> ProjectReport { get; set; }
        public virtual ICollection<Study> Study { get; set; }
    }
}
