using System;
using System.Collections.Generic;

namespace SQT.Infrastructure.Model
{
    public partial class TblDatasetSubmissionTypes
    {
        public TblDatasetSubmissionTypes()
        {
            TblDatasetSubmissions = new HashSet<TblDatasetSubmissions>();
        }

        public int SubmissionTypeId { get; set; }
        public string SubmissionType { get; set; }
        public string Description { get; set; }
        public DateTime? DateUpdated { get; set; }

        public virtual ICollection<TblDatasetSubmissions> TblDatasetSubmissions { get; set; }
    }
}
