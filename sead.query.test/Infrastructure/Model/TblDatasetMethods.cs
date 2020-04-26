using System;
using System.Collections.Generic;

namespace SQT.Infrastructure.Model
{
    public partial class TblDatasetMethods
    {
        public int DatasetMethodId { get; set; }
        public int DatasetId { get; set; }
        public int MethodId { get; set; }
        public DateTime? DateUpdated { get; set; }

        public virtual TblDatasets Dataset { get; set; }
        public virtual TblMethods Method { get; set; }
    }
}
