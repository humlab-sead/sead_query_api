using System;
using System.Collections.Generic;

namespace SQT.Infrastructure.Model
{
    public partial class TblCeramics
    {
        public int CeramicsId { get; set; }
        public int AnalysisEntityId { get; set; }
        public string MeasurementValue { get; set; }
        public DateTime? DateUpdated { get; set; }
        public int CeramicsLookupId { get; set; }

        public virtual TblAnalysisEntities AnalysisEntity { get; set; }
        public virtual TblCeramicsLookup CeramicsLookup { get; set; }
    }
}
