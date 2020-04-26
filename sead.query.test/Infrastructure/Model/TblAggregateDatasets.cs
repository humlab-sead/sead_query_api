using System;
using System.Collections.Generic;

namespace SQT.Infrastructure.Model
{
    public partial class TblAggregateDatasets
    {
        public TblAggregateDatasets()
        {
            TblAggregateSampleAges = new HashSet<TblAggregateSampleAges>();
            TblAggregateSamples = new HashSet<TblAggregateSamples>();
        }

        public int AggregateDatasetId { get; set; }
        public int AggregateOrderTypeId { get; set; }
        public int? BiblioId { get; set; }
        public string AggregateDatasetName { get; set; }
        public DateTime? DateUpdated { get; set; }
        public string Description { get; set; }

        public virtual TblAggregateOrderTypes AggregateOrderType { get; set; }
        public virtual TblBiblio Biblio { get; set; }
        public virtual ICollection<TblAggregateSampleAges> TblAggregateSampleAges { get; set; }
        public virtual ICollection<TblAggregateSamples> TblAggregateSamples { get; set; }
    }
}
