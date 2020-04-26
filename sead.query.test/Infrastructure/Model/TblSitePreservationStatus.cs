using System;
using System.Collections.Generic;

namespace SQT.Infrastructure.Model
{
    public partial class TblSitePreservationStatus
    {
        public int SitePreservationStatusId { get; set; }
        public int? SiteId { get; set; }
        public string PreservationStatusOrThreat { get; set; }
        public string Description { get; set; }
        public string AssessmentType { get; set; }
        public int? AssessmentAuthorContactId { get; set; }
        public DateTime? DateUpdated { get; set; }
        public DateTime? EvaluationDate { get; set; }

        public virtual TblSites Site { get; set; }
    }
}
