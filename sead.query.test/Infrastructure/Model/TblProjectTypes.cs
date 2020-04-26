using System;
using System.Collections.Generic;

namespace SQT.Infrastructure.Model
{
    public partial class TblProjectTypes
    {
        public TblProjectTypes()
        {
            TblProjects = new HashSet<TblProjects>();
        }

        public int ProjectTypeId { get; set; }
        public string ProjectTypeName { get; set; }
        public string Description { get; set; }
        public DateTime? DateUpdated { get; set; }

        public virtual ICollection<TblProjects> TblProjects { get; set; }
    }
}
