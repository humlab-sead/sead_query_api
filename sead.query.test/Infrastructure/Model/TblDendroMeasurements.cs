using System;
using System.Collections.Generic;

namespace SQT.Infrastructure.Model
{
    public partial class TblDendroMeasurements
    {
        public int DendroMeasurementId { get; set; }
        public DateTime? DateUpdated { get; set; }
        public int? MethodId { get; set; }

        public virtual TblMethods Method { get; set; }
    }
}
