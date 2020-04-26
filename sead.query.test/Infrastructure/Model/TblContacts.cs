using System;
using System.Collections.Generic;

namespace SQT.Infrastructure.Model
{
    public partial class TblContacts
    {
        public TblContacts()
        {
            TblChronologies = new HashSet<TblChronologies>();
            TblDatasetContacts = new HashSet<TblDatasetContacts>();
            TblDatasetMasters = new HashSet<TblDatasetMasters>();
            TblDatasetSubmissions = new HashSet<TblDatasetSubmissions>();
            TblDatingLabs = new HashSet<TblDatingLabs>();
            TblSiteImages = new HashSet<TblSiteImages>();
            TblTaxaReferenceSpecimens = new HashSet<TblTaxaReferenceSpecimens>();
        }

        public int ContactId { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public int? LocationId { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Url { get; set; }
        public DateTime? DateUpdated { get; set; }

        public virtual ICollection<TblChronologies> TblChronologies { get; set; }
        public virtual ICollection<TblDatasetContacts> TblDatasetContacts { get; set; }
        public virtual ICollection<TblDatasetMasters> TblDatasetMasters { get; set; }
        public virtual ICollection<TblDatasetSubmissions> TblDatasetSubmissions { get; set; }
        public virtual ICollection<TblDatingLabs> TblDatingLabs { get; set; }
        public virtual ICollection<TblSiteImages> TblSiteImages { get; set; }
        public virtual ICollection<TblTaxaReferenceSpecimens> TblTaxaReferenceSpecimens { get; set; }
    }
}
