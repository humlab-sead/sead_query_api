using System;
using System.Collections.Generic;

namespace SQT.Infrastructure.Model
{
    public partial class TblEcocodeDefinitions
    {
        public TblEcocodeDefinitions()
        {
            TblEcocodes = new HashSet<TblEcocodes>();
        }

        public int EcocodeDefinitionId { get; set; }
        public string Abbreviation { get; set; }
        public DateTime? DateUpdated { get; set; }
        public string Definition { get; set; }
        public int? EcocodeGroupId { get; set; }
        public string Name { get; set; }
        public string Notes { get; set; }
        public short? SortOrder { get; set; }

        public virtual TblEcocodeGroups EcocodeGroup { get; set; }
        public virtual ICollection<TblEcocodes> TblEcocodes { get; set; }
    }
}
