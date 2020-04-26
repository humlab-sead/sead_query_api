using System;
using System.Collections.Generic;

namespace SQT.Infrastructure.Model
{
    public partial class TblIsotopes
    {
        public int IsotopeId { get; set; }
        public int AnalysisEntityId { get; set; }
        public int IsotopeMeasurementId { get; set; }
        public int? IsotopeStandardId { get; set; }
        public string MeasurementValue { get; set; }
        public int UnitId { get; set; }
        public int IsotopeValueSpecifierId { get; set; }
        public DateTime? DateUpdated { get; set; }

        public virtual TblAnalysisEntities AnalysisEntity { get; set; }
        public virtual TblIsotopeMeasurements IsotopeMeasurement { get; set; }
        public virtual TblIsotopeStandards IsotopeStandard { get; set; }
        public virtual TblIsotopeValueSpecifiers IsotopeValueSpecifier { get; set; }
        public virtual TblUnits Unit { get; set; }
    }
}
