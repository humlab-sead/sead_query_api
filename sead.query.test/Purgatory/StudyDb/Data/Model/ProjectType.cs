using System;
using System.Collections.Generic;

namespace SeadQueryTest.Infrastructure.Data.StudyModel.Model
{
    public partial class ProjectType
    {
        public ProjectType()
        {
            Project = new HashSet<Project>();
        }

        public int ProjectTypeId { get; set; }
        public string TypeName { get; set; }

        public virtual ICollection<Project> Project { get; set; }
    }
}
