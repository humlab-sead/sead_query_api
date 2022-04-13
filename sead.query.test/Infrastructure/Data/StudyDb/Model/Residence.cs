using System;
using System.Collections.Generic;

namespace SeadQueryTest.Infrastructure.Data.StudyModel.Model
{
    public partial class Residence
    {
        public Residence()
        {
            ProjectResidence = new HashSet<ProjectResidence>();
        }

        public int ResidenceId { get; set; }
        public string Name { get; set; }
        public int ResidenceTypeId { get; set; }

        public virtual ResidenceType ResidenceType { get; set; }
        public virtual ICollection<ProjectResidence> ProjectResidence { get; set; }
    }
}
