using System;
using System.Collections.Generic;

namespace SQT.Infrastructure.Model
{
    public partial class TblDataTypeGroups
    {
        public TblDataTypeGroups()
        {
            TblDataTypes = new HashSet<TblDataTypes>();
        }

        public int DataTypeGroupId { get; set; }
        public string DataTypeGroupName { get; set; }
        public DateTime? DateUpdated { get; set; }
        public string Description { get; set; }

        public virtual ICollection<TblDataTypes> TblDataTypes { get; set; }
    }
}
