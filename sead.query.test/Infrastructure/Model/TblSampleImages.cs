using System;
using System.Collections.Generic;

namespace SQT.Infrastructure.Model
{
    public partial class TblSampleImages
    {
        public int SampleImageId { get; set; }
        public DateTime? DateUpdated { get; set; }
        public string Description { get; set; }
        public string ImageLocation { get; set; }
        public string ImageName { get; set; }
        public int ImageTypeId { get; set; }
        public int PhysicalSampleId { get; set; }

        public virtual TblImageTypes ImageType { get; set; }
        public virtual TblPhysicalSamples PhysicalSample { get; set; }
    }
}
