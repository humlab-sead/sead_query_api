using System;
using System.Collections.Generic;

namespace SQT.Infrastructure.Model
{
    public partial class TblSites
    {
        public TblSites()
        {
            TblSampleGroups = new HashSet<TblSampleGroups>();
            TblSiteImages = new HashSet<TblSiteImages>();
            TblSiteLocations = new HashSet<TblSiteLocations>();
            TblSiteNatgridrefs = new HashSet<TblSiteNatgridrefs>();
            TblSiteOtherRecords = new HashSet<TblSiteOtherRecords>();
            TblSitePreservationStatus = new HashSet<TblSitePreservationStatus>();
            TblSiteReferences = new HashSet<TblSiteReferences>();
        }

        public int SiteId { get; set; }
        public decimal? Altitude { get; set; }
        public decimal? LatitudeDd { get; set; }
        public decimal? LongitudeDd { get; set; }
        public string NationalSiteIdentifier { get; set; }
        public string SiteDescription { get; set; }
        public string SiteName { get; set; }
        public int? SitePreservationStatusId { get; set; }
        public DateTime? DateUpdated { get; set; }
        public string SiteLocationAccuracy { get; set; }

        public virtual ICollection<TblSampleGroups> TblSampleGroups { get; set; }
        public virtual ICollection<TblSiteImages> TblSiteImages { get; set; }
        public virtual ICollection<TblSiteLocations> TblSiteLocations { get; set; }
        public virtual ICollection<TblSiteNatgridrefs> TblSiteNatgridrefs { get; set; }
        public virtual ICollection<TblSiteOtherRecords> TblSiteOtherRecords { get; set; }
        public virtual ICollection<TblSitePreservationStatus> TblSitePreservationStatus { get; set; }
        public virtual ICollection<TblSiteReferences> TblSiteReferences { get; set; }
    }
}
