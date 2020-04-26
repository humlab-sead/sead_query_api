using System;
using System.Collections.Generic;

namespace SQT.Infrastructure.Model
{
    public partial class TblAggregateSamples
    {
        public int AggregateSampleId { get; set; }
        public int AggregateDatasetId { get; set; }
        public int AnalysisEntityId { get; set; }
        public string AggregateSampleName { get; set; }
        public DateTime? DateUpdated { get; set; }

        public virtual TblAggregateDatasets AggregateDataset { get; set; }
        public virtual TblAnalysisEntities AnalysisEntity { get; set; }
    }
}
