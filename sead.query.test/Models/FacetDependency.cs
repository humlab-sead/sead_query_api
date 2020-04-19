using System;
using System.Collections.Generic;

namespace SQT.Models
{
    public partial class FacetDependency
    {
        public int FacetDependencyId { get; set; }
        public int FacetId { get; set; }
        public int DependencyFacetId { get; set; }

        public virtual Facet DependencyFacet { get; set; }
        public virtual Facet Facet { get; set; }
    }
}
