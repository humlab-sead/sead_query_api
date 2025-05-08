using System;
using System.Collections.Generic;

namespace SeadQueryTest.Infrastructure.Data.StudyModel.Model
{
    public partial class ObservedSpan
    {
        public int ObservedSpanId { get; set; }
        public decimal? Low { get; set; }
        public decimal? High { get; set; }
        public int ObservableId { get; set; }
        public int ExperimentId { get; set; }

        public virtual Experiment Experiment { get; set; }
        public virtual Observable Observable { get; set; }
    }
}
