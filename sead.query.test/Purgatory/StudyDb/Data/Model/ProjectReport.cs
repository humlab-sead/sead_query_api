using System;
using System.Collections.Generic;

namespace SeadQueryTest.Infrastructure.Data.StudyModel.Model
{
    public partial class ProjectReport
    {
        public int ProjectReportId { get; set; }
        public int? ProjectId { get; set; }
        public int? ReportId { get; set; }

        public virtual Project Project { get; set; }
        public virtual Report Report { get; set; }
    }
}
