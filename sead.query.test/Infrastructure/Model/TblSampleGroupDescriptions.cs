using System;
using System.Collections.Generic;

namespace SQT.Infrastructure.Model
{
    public partial class TblSampleGroupDescriptions
    {
        public int SampleGroupDescriptionId { get; set; }
        public string GroupDescription { get; set; }
        public int SampleGroupDescriptionTypeId { get; set; }
        public DateTime? DateUpdated { get; set; }
        public int? SampleGroupId { get; set; }

        public virtual TblSampleGroups SampleGroup { get; set; }
        public virtual TblSampleGroupDescriptionTypes SampleGroupDescriptionType { get; set; }
    }
}
