using System;
using System.Collections.Generic;

namespace SQT.Infrastructure.Model
{
    public partial class TblChronControls
    {
        public int ChronControlId { get; set; }
        public decimal? Age { get; set; }
        public decimal? AgeLimitOlder { get; set; }
        public decimal? AgeLimitYounger { get; set; }
        public int? ChronControlTypeId { get; set; }
        public int ChronologyId { get; set; }
        public DateTime? DateUpdated { get; set; }
        public decimal? DepthBottom { get; set; }
        public decimal? DepthTop { get; set; }
        public string Notes { get; set; }

        public virtual TblChronControlTypes ChronControlType { get; set; }
        public virtual TblChronologies Chronology { get; set; }
    }
}
