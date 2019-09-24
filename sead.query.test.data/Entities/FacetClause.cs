using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace sead.query.test.data.Entities
{
    public partial class FacetClause
    {
        public int FacetSourceTableId { get; set; }
        public int FacetId { get; set; }
        public string Clause { get; set; }

        [JsonIgnore]
        public virtual Facet Facet { get; set; }
    }
}
