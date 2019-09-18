using Newtonsoft.Json;

namespace SeadQueryCore
{

    /// <summary>
    /// Condition clauses associated to a facet
    /// </summary>
    public class FacetClause {

        public int FacetSourceTableId { get; set; }
        public int FacetId { get; set; }
        public virtual string Clause { get; set; }

        [JsonIgnore] public Facet Facet { get; set; }
    }
}
