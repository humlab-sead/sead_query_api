using System;
using System.Collections.Generic;

namespace SeadQueryTest.Infrastructure.Data.StudyModel.Model
{
    public partial class Method
    {
        public Method()
        {
            Cohort = new HashSet<Cohort>();
            Study = new HashSet<Study>();
        }

        public int MethodId { get; set; }
        public int ReportId { get; set; }
        public string Description { get; set; }
        public string MethodName { get; set; }
        public int? MethodTypeId { get; set; }

        public virtual MethodType MethodType { get; set; }
        public virtual Report Report { get; set; }
        public virtual ICollection<Cohort> Cohort { get; set; }
        public virtual ICollection<Study> Study { get; set; }
    }
}
