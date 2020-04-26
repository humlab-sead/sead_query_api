using System;
using System.Collections.Generic;

namespace SQT.Infrastructure.Model
{
    public partial class TblIsotopeTypes
    {
        public TblIsotopeTypes()
        {
            TblIsotopeMeasurements = new HashSet<TblIsotopeMeasurements>();
        }

        public int IsotopeTypeId { get; set; }
        public string Designation { get; set; }
        public string Abbreviation { get; set; }
        public decimal? AtomicNumber { get; set; }
        public string Description { get; set; }
        public string AlternativeDesignation { get; set; }
        public DateTime? DateUpdated { get; set; }

        public virtual ICollection<TblIsotopeMeasurements> TblIsotopeMeasurements { get; set; }
    }
}
