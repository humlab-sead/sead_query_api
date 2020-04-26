using System;
using System.Collections.Generic;

namespace SQT.Infrastructure.Model
{
    public partial class TblAggregateSampleAges
    {
        public int AggregateSampleAgeId { get; set; }
        public int AggregateDatasetId { get; set; }
        public int AnalysisEntityAgeId { get; set; }
        public DateTime? DateUpdated { get; set; }

        public virtual TblAggregateDatasets AggregateDataset { get; set; }
        public virtual TblAnalysisEntityAges AnalysisEntityAge { get; set; }
    }
}
