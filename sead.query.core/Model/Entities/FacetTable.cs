using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace SeadQueryCore
{

    /// <summary>
    /// A relational table associated to a facet
    /// </summary>
    public class FacetTable {

        public int FacetTableId { get; set; }
        public int FacetId { get; set; }
        public int SequenceId { get; set; }
        public string SchemaName { get; set; }
        public string ObjectName { get; set; }
        public string ObjectArgs { get; set; }
        public string Alias { get; set; }

        [JsonIgnore]
        public virtual Facet Facet { get; set; }
    }
}
