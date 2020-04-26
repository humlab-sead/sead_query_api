using System;
using System.Collections.Generic;

namespace SQT.Infrastructure.Model
{
    public partial class TblDatasetContacts
    {
        public int DatasetContactId { get; set; }
        public int ContactId { get; set; }
        public int ContactTypeId { get; set; }
        public int DatasetId { get; set; }
        public DateTime? DateUpdated { get; set; }

        public virtual TblContacts Contact { get; set; }
        public virtual TblContactTypes ContactType { get; set; }
        public virtual TblDatasets Dataset { get; set; }
    }
}
