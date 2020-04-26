using System;
using System.Collections.Generic;

namespace SQT.Infrastructure.Model
{
    public partial class TblDataTypes
    {
        public TblDataTypes()
        {
            TblDatasets = new HashSet<TblDatasets>();
        }

        public int DataTypeId { get; set; }
        public int DataTypeGroupId { get; set; }
        public string DataTypeName { get; set; }
        public DateTime? DateUpdated { get; set; }
        public string Definition { get; set; }

        public virtual TblDataTypeGroups DataTypeGroup { get; set; }
        public virtual ICollection<TblDatasets> TblDatasets { get; set; }
    }
}
