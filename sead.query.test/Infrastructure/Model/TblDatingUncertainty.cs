using System;
using System.Collections.Generic;

namespace SQT.Infrastructure.Model
{
    public partial class TblDatingUncertainty
    {
        public TblDatingUncertainty()
        {
            TblDendroDates = new HashSet<TblDendroDates>();
            TblGeochronology = new HashSet<TblGeochronology>();
            TblRelativeDates = new HashSet<TblRelativeDates>();
            TblTephraDates = new HashSet<TblTephraDates>();
        }

        public int DatingUncertaintyId { get; set; }
        public DateTime? DateUpdated { get; set; }
        public string Description { get; set; }
        public string Uncertainty { get; set; }

        public virtual ICollection<TblDendroDates> TblDendroDates { get; set; }
        public virtual ICollection<TblGeochronology> TblGeochronology { get; set; }
        public virtual ICollection<TblRelativeDates> TblRelativeDates { get; set; }
        public virtual ICollection<TblTephraDates> TblTephraDates { get; set; }
    }
}
