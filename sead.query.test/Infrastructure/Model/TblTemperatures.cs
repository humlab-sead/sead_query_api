using System;
using System.Collections.Generic;

namespace SQT.Infrastructure.Model
{
    public partial class TblTemperatures
    {
        public int RecordId { get; set; }
        public int YearsBp { get; set; }
        public decimal? D180Gisp2 { get; set; }
    }
}
