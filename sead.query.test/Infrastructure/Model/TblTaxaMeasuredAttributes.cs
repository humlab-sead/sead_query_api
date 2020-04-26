using System;
using System.Collections.Generic;

namespace SQT.Infrastructure.Model
{
    public partial class TblTaxaMeasuredAttributes
    {
        public int MeasuredAttributeId { get; set; }
        public string AttributeMeasure { get; set; }
        public string AttributeType { get; set; }
        public string AttributeUnits { get; set; }
        public decimal? Data { get; set; }
        public DateTime? DateUpdated { get; set; }
        public int TaxonId { get; set; }

        public virtual TblTaxaTreeMaster Taxon { get; set; }
    }
}
