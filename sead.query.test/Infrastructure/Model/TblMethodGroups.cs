using System;
using System.Collections.Generic;

namespace SQT.Infrastructure.Model
{
    public partial class TblMethodGroups
    {
        public TblMethodGroups()
        {
            TblDimensions = new HashSet<TblDimensions>();
            TblMethods = new HashSet<TblMethods>();
        }

        public int MethodGroupId { get; set; }
        public DateTime? DateUpdated { get; set; }
        public string Description { get; set; }
        public string GroupName { get; set; }

        public virtual ICollection<TblDimensions> TblDimensions { get; set; }
        public virtual ICollection<TblMethods> TblMethods { get; set; }
    }
}
