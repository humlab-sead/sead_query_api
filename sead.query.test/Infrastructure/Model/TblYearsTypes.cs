using System;
using System.Collections.Generic;

namespace SQT.Infrastructure.Model
{
    public partial class TblYearsTypes
    {
        public int YearsTypeId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime? DateUpdated { get; set; }
    }
}
