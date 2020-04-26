using System;
using System.Collections.Generic;

namespace SQT.Infrastructure.Model
{
    public partial class TblDatingLabs
    {
        public TblDatingLabs()
        {
            TblGeochronology = new HashSet<TblGeochronology>();
        }

        public int DatingLabId { get; set; }
        public int? ContactId { get; set; }
        public string InternationalLabId { get; set; }
        public string LabName { get; set; }
        public int? CountryId { get; set; }
        public DateTime? DateUpdated { get; set; }

        public virtual TblContacts Contact { get; set; }
        public virtual ICollection<TblGeochronology> TblGeochronology { get; set; }
    }
}
