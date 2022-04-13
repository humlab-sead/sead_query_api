using System;
using System.Collections.Generic;

namespace SeadQueryTest.Infrastructure.Data.StudyModel.Model
{
    public partial class ObservedMagnitude
    {
        public int ObservedMagnitudeId { get; set; }
        public int ExperimentId { get; set; }
        public int ObservableId { get; set; }
        public decimal Value { get; set; }

        public virtual Experiment Experiment { get; set; }
        public virtual Observable Observable { get; set; }
    }
}
