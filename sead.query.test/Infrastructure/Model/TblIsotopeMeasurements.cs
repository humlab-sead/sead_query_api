using System;
using System.Collections.Generic;

namespace SQT.Infrastructure.Model
{
    public partial class TblIsotopeMeasurements
    {
        public TblIsotopeMeasurements()
        {
            TblIsotopes = new HashSet<TblIsotopes>();
        }

        public int IsotopeMeasurementId { get; set; }
        public int? IsotopeStandardId { get; set; }
        public int? MethodId { get; set; }
        public int? IsotopeTypeId { get; set; }
        public DateTime? DateUpdated { get; set; }

        public virtual TblIsotopeStandards IsotopeStandard { get; set; }
        public virtual TblIsotopeTypes IsotopeType { get; set; }
        public virtual TblMethods Method { get; set; }
        public virtual ICollection<TblIsotopes> TblIsotopes { get; set; }
    }
}
