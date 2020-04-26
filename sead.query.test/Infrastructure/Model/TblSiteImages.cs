using System;
using System.Collections.Generic;

namespace SQT.Infrastructure.Model
{
    public partial class TblSiteImages
    {
        public int SiteImageId { get; set; }
        public int? ContactId { get; set; }
        public string Credit { get; set; }
        public DateTime? DateTaken { get; set; }
        public DateTime? DateUpdated { get; set; }
        public string Description { get; set; }
        public string ImageLocation { get; set; }
        public string ImageName { get; set; }
        public int ImageTypeId { get; set; }
        public int SiteId { get; set; }

        public virtual TblContacts Contact { get; set; }
        public virtual TblImageTypes ImageType { get; set; }
        public virtual TblSites Site { get; set; }
    }
}
