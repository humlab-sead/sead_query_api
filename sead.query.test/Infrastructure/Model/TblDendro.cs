using System;
using System.Collections.Generic;

namespace SQT.Infrastructure.Model
{
    public partial class TblDendro
    {
        public int DendroId { get; set; }
        public int AnalysisEntityId { get; set; }
        public string MeasurementValue { get; set; }
        public DateTime? DateUpdated { get; set; }
        public int DendroLookupId { get; set; }

        public virtual TblAnalysisEntities AnalysisEntity { get; set; }
        public virtual TblDendroLookup DendroLookup { get; set; }
    }
}
