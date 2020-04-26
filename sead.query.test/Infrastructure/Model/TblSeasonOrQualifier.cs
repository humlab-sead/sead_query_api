using System;
using System.Collections.Generic;

namespace SQT.Infrastructure.Model
{
    public partial class TblSeasonOrQualifier
    {
        public int SeasonOrQualifierId { get; set; }
        public string SeasonOrQualifierType { get; set; }
        public string Description { get; set; }
        public DateTime? DateUpdated { get; set; }
    }
}
