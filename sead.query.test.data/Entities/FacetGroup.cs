using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace sead.query.test.data.Entities
{
    public partial class FacetGroup
    {
        public FacetGroup()
        {
            Facets = new HashSet<Facet>();
        }

        public int FacetGroupId { get; set; }
        public string FacetGroupKey { get; set; }
        public string DisplayTitle { get; set; }
        public bool IsApplicable { get; set; }
        public bool IsDefault { get; set; }

        [JsonIgnore]
        public virtual ICollection<Facet> Facets { get; set; }
    }
}
