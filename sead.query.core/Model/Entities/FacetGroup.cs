using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace SeadQueryCore
{

    /// <summary>
    /// Facet definition group type
    /// </summary>
    public class FacetGroup
    {
        public FacetGroup()
        {
            //Facets = new HashSet<Facet>();
        }

        [JsonIgnore]
        public int FacetGroupId { get; set; }

        public string FacetGroupKey { get; set; }
        public string DisplayTitle { get; set; }
        public string Description { get; set; }
        [JsonIgnore]
        public bool IsApplicable { get; set; }
        [JsonIgnore]
        public bool IsDefault { get; set; }

        //[JsonIgnore]
        //public ICollection<Facet> Facets { get; set; }
    }
}
