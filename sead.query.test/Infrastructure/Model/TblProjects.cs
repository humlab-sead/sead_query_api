using System;
using System.Collections.Generic;

namespace SQT.Infrastructure.Model
{
    public partial class TblProjects
    {
        public TblProjects()
        {
            TblDatasets = new HashSet<TblDatasets>();
        }

        public int ProjectId { get; set; }
        public int? ProjectTypeId { get; set; }
        public int? ProjectStageId { get; set; }
        public string ProjectName { get; set; }
        public string ProjectAbbrevName { get; set; }
        public string Description { get; set; }
        public DateTime? DateUpdated { get; set; }

        public virtual TblProjectStages ProjectStage { get; set; }
        public virtual TblProjectTypes ProjectType { get; set; }
        public virtual ICollection<TblDatasets> TblDatasets { get; set; }
    }
}
