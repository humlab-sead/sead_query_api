using System;
using System.Collections.Generic;

namespace SQT.Infrastructure.Model
{
    public partial class TblImageTypes
    {
        public TblImageTypes()
        {
            TblSampleGroupImages = new HashSet<TblSampleGroupImages>();
            TblSampleImages = new HashSet<TblSampleImages>();
            TblSiteImages = new HashSet<TblSiteImages>();
            TblTaxaImages = new HashSet<TblTaxaImages>();
        }

        public int ImageTypeId { get; set; }
        public DateTime? DateUpdated { get; set; }
        public string Description { get; set; }
        public string ImageType { get; set; }

        public virtual ICollection<TblSampleGroupImages> TblSampleGroupImages { get; set; }
        public virtual ICollection<TblSampleImages> TblSampleImages { get; set; }
        public virtual ICollection<TblSiteImages> TblSiteImages { get; set; }
        public virtual ICollection<TblTaxaImages> TblTaxaImages { get; set; }
    }
}
