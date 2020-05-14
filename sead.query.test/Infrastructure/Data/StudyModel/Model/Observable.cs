using System;
using System.Collections.Generic;

namespace SeadQueryTest.Infrastructure.Data.StudyModel.Model
{
    public partial class Observable
    {
        public Observable()
        {
            ObservedCount = new HashSet<ObservedCount>();
            ObservedMagnitude = new HashSet<ObservedMagnitude>();
            ObservedSpan = new HashSet<ObservedSpan>();
        }

        public int ObservableId { get; set; }
        public decimal Name { get; set; }

        public virtual ICollection<ObservedCount> ObservedCount { get; set; }
        public virtual ICollection<ObservedMagnitude> ObservedMagnitude { get; set; }
        public virtual ICollection<ObservedSpan> ObservedSpan { get; set; }
    }
}
