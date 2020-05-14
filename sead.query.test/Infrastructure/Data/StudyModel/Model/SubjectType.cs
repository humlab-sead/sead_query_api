using System;
using System.Collections.Generic;

namespace SeadQueryTest.Infrastructure.Data.StudyModel.Model
{
    public partial class SubjectType
    {
        public SubjectType()
        {
            Subject = new HashSet<Subject>();
        }

        public int SubjectTypeId { get; set; }
        public string Description { get; set; }

        public virtual ICollection<Subject> Subject { get; set; }
    }
}
