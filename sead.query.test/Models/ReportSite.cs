using System;
using System.Collections.Generic;

namespace SeadQueryTest.Models
{
    public partial class ReportSite
    {
        public int? Id { get; set; }
        public int? SiteId { get; set; }
        public string SiteName { get; set; }
        public string SiteDescription { get; set; }
        public string NationalGridRef { get; set; }
        public string Places { get; set; }
        public string PreservationStatusOrThreat { get; set; }
        public decimal? SiteLat { get; set; }
        public decimal? SiteLng { get; set; }
    }
}
