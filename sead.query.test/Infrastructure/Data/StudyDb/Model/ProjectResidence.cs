using System;
using System.Collections.Generic;

namespace SeadQueryTest.Infrastructure.Data.StudyModel.Model
{
    public partial class ProjectResidence
    {
        public int ProjectResidenceId { get; set; }
        public int ResidenceId { get; set; }
        public decimal? LatitudeDd { get; set; }
        public decimal? LongitudeDd { get; set; }
        public int? ProjectId { get; set; }

        public virtual Project Project { get; set; }
        public virtual Residence Residence { get; set; }
    }
}
