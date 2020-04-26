using System;
using System.Collections.Generic;

namespace SQT.Infrastructure.Model
{
    public partial class TblIsotopeStandards
    {
        public TblIsotopeStandards()
        {
            TblIsotopeMeasurements = new HashSet<TblIsotopeMeasurements>();
            TblIsotopes = new HashSet<TblIsotopes>();
        }

        public int IsotopeStandardId { get; set; }
        public string IsotopeRation { get; set; }
        public string InternationalScale { get; set; }
        public string AcceptedRatioXe6 { get; set; }
        public string ErrorOfRatio { get; set; }
        public string Reference { get; set; }
        public DateTime? DateUpdated { get; set; }

        public virtual ICollection<TblIsotopeMeasurements> TblIsotopeMeasurements { get; set; }
        public virtual ICollection<TblIsotopes> TblIsotopes { get; set; }
    }
}
