using System;
using System.Collections.Generic;

namespace SQT.Models
{
    public partial class FacetChildren
    {
        public string FacetCode { get; set; }
        public string ChildFacetCode { get; set; }
        public int Position { get; set; }

        public virtual Facet ChildFacetCodeNavigation { get; set; }
        public virtual Facet FacetCodeNavigation { get; set; }
    }
}
