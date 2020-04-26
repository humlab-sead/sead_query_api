using System;
using System.Collections.Generic;

namespace SQT.Infrastructure.Model
{
    public partial class TblTaxaImages
    {
        public int TaxaImagesId { get; set; }
        public string ImageName { get; set; }
        public string Description { get; set; }
        public string ImageLocation { get; set; }
        public int? ImageTypeId { get; set; }
        public int TaxonId { get; set; }
        public DateTime? DateUpdated { get; set; }

        public virtual TblImageTypes ImageType { get; set; }
        public virtual TblTaxaTreeMaster Taxon { get; set; }
    }
}
