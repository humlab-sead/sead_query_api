using System;
using System.Collections.Generic;

namespace SQT.Infrastructure.Model
{
    public partial class TblEcocodes
    {
        public int EcocodeId { get; set; }
        public DateTime? DateUpdated { get; set; }
        public int? EcocodeDefinitionId { get; set; }
        public int? TaxonId { get; set; }

        public virtual TblEcocodeDefinitions EcocodeDefinition { get; set; }
        public virtual TblTaxaTreeMaster Taxon { get; set; }
    }
}
