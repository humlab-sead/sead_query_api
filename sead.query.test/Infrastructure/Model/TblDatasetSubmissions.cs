using System;
using System.Collections.Generic;

namespace SQT.Infrastructure.Model
{
    public partial class TblDatasetSubmissions
    {
        public int DatasetSubmissionId { get; set; }
        public int DatasetId { get; set; }
        public int SubmissionTypeId { get; set; }
        public int ContactId { get; set; }
        public DateTime DateSubmitted { get; set; }
        public string Notes { get; set; }
        public DateTime? DateUpdated { get; set; }

        public virtual TblContacts Contact { get; set; }
        public virtual TblDatasets Dataset { get; set; }
        public virtual TblDatasetSubmissionTypes SubmissionType { get; set; }
    }
}
