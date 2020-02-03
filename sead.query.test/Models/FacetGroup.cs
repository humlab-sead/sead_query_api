using System;
using System.Collections.Generic;

namespace SeadQueryTest.Models
{
    public partial class FacetGroup
    {
        public FacetGroup()
        {
            Facet = new HashSet<Facet>();
        }

        public int FacetGroupId { get; set; }
        public string FacetGroupKey { get; set; }
        public string DisplayTitle { get; set; }
        public string Description { get; set; }
        public bool IsApplicable { get; set; }
        public bool IsDefault { get; set; }

        public virtual ICollection<Facet> Facet { get; set; }
    }
}
