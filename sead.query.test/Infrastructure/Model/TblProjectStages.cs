using System;
using System.Collections.Generic;

namespace SQT.Infrastructure.Model
{
    public partial class TblProjectStages
    {
        public TblProjectStages()
        {
            TblProjects = new HashSet<TblProjects>();
        }

        public int ProjectStageId { get; set; }
        public string StageName { get; set; }
        public string Description { get; set; }
        public DateTime? DateUpdated { get; set; }

        public virtual ICollection<TblProjects> TblProjects { get; set; }
    }
}
