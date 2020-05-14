using System;
using System.Collections.Generic;

namespace SeadQueryTest.Infrastructure.Data.StudyModel.Model
{
    public partial class StudyType
    {
        public StudyType()
        {
            Study = new HashSet<Study>();
        }

        public int StudyTypeId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public virtual ICollection<Study> Study { get; set; }
    }
}
