using System;
using System.Collections.Generic;

namespace SQT.Infrastructure.Model
{
    public partial class TblCeramicsMeasurements
    {
        public int CeramicsMeasurementId { get; set; }
        public DateTime? DateUpdated { get; set; }
        public int? MethodId { get; set; }

        public virtual TblMethods Method { get; set; }
    }
}
