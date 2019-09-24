using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace sead.query.test.data.Entities
{
    public partial class FacetTable
    {
        public int FacetTableId { get; set; }
        public int FacetId { get; set; }
        public int SequenceId { get; set; }
        public string SchemaName { get; set; }
        public string TableName { get; set; }
        public string Alias { get; set; }

        [JsonIgnore]
        public virtual Facet Facet { get; set; }
    }
}
