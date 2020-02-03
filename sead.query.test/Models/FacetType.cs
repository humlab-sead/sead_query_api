using System;
using System.Collections.Generic;

namespace SeadQueryTest.Models
{
    public partial class FacetType
    {
        public FacetType()
        {
            Facet = new HashSet<Facet>();
        }

        public int FacetTypeId { get; set; }
        public string FacetTypeName { get; set; }
        public bool ReloadAsTarget { get; set; }

        public virtual ICollection<Facet> Facet { get; set; }
    }
}
