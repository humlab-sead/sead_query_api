using System;
using System.Collections.Generic;

namespace SQT.Models
{
    public partial class FacetClause
    {
        public int FacetClauseId { get; set; }
        public int FacetId { get; set; }
        public string Clause { get; set; }

        public virtual Facet Facet { get; set; }
    }
}
