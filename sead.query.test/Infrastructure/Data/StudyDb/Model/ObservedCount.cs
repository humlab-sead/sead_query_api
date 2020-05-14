using System;
using System.Collections.Generic;

namespace SeadQueryTest.Infrastructure.Data.StudyModel.Model
{
    public partial class ObservedCount
    {
        public int ObservedCountId { get; set; }
        public int ExperimentId { get; set; }
        public int ObservableId { get; set; }
        public int Count { get; set; }

        public virtual Experiment Experiment { get; set; }
        public virtual Observable Observable { get; set; }
    }
}
