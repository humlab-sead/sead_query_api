using System;
using System.Collections.Generic;

namespace SQT.Infrastructure.Model
{
    public partial class TblMcrdataBirmbeetledat
    {
        public int McrdataBirmbeetledatId { get; set; }
        public DateTime? DateUpdated { get; set; }
        public string McrData { get; set; }
        public short McrRow { get; set; }
        public int TaxonId { get; set; }

        public virtual TblTaxaTreeMaster Taxon { get; set; }
    }
}
