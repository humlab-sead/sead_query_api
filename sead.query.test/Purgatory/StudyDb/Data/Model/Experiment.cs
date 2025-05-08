using System;
using System.Collections.Generic;

namespace SeadQueryTest.Infrastructure.Data.StudyModel.Model
{
    public partial class Experiment
    {
        public Experiment()
        {
            ObservedCount = new HashSet<ObservedCount>();
            ObservedMagnitude = new HashSet<ObservedMagnitude>();
            ObservedSpan = new HashSet<ObservedSpan>();
        }

        public int ExperimentId { get; set; }
        public int SubjectId { get; set; }
        public int StudyId { get; set; }

        public virtual Study Study { get; set; }
        public virtual Subject Subject { get; set; }
        public virtual ICollection<ObservedCount> ObservedCount { get; set; }
        public virtual ICollection<ObservedMagnitude> ObservedMagnitude { get; set; }
        public virtual ICollection<ObservedSpan> ObservedSpan { get; set; }
    }
}
