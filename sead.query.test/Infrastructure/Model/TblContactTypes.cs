using System;
using System.Collections.Generic;

namespace SQT.Infrastructure.Model
{
    public partial class TblContactTypes
    {
        public TblContactTypes()
        {
            TblDatasetContacts = new HashSet<TblDatasetContacts>();
        }

        public int ContactTypeId { get; set; }
        public string ContactTypeName { get; set; }
        public DateTime? DateUpdated { get; set; }
        public string Description { get; set; }

        public virtual ICollection<TblDatasetContacts> TblDatasetContacts { get; set; }
    }
}
