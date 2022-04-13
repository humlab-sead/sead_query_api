using System;
using System.Collections.Generic;

namespace SeadQueryTest.Infrastructure.Data.StudyModel.Model
{
    public partial class ResidenceType
    {
        public ResidenceType()
        {
            Residence = new HashSet<Residence>();
        }

        public int ResidenceTypeId { get; set; }
        public string Description { get; set; }

        public virtual ICollection<Residence> Residence { get; set; }
    }
}
