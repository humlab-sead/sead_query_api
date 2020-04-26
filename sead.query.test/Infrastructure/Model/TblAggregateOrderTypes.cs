using System;
using System.Collections.Generic;

namespace SQT.Infrastructure.Model
{
    public partial class TblAggregateOrderTypes
    {
        public TblAggregateOrderTypes()
        {
            TblAggregateDatasets = new HashSet<TblAggregateDatasets>();
        }

        public int AggregateOrderTypeId { get; set; }
        public string AggregateOrderType { get; set; }
        public DateTime? DateUpdated { get; set; }
        public string Description { get; set; }

        public virtual ICollection<TblAggregateDatasets> TblAggregateDatasets { get; set; }
    }
}
