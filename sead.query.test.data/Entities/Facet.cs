using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace sead.query.test.data.Entities
{
    public partial class Facet
    {
        public Facet()
        {
            Clauses = new HashSet<FacetClause>();
            Tables = new HashSet<FacetTable>();
        }

        public int FacetId { get; set; }
        public string FacetCode { get; set; }
        public string DisplayTitle { get; set; }
        public int FacetGroupId { get; set; }
        public int FacetTypeId { get; set; }
        public string CategoryIdExpr { get; set; }
        public string CategoryNameExpr { get; set; }
        public string SortExpr { get; set; }
        public bool IsApplicable { get; set; }
        public bool IsDefault { get; set; }
        public string AggregateType { get; set; }
        public string AggregateTitle { get; set; }
        public int AggregateFacetId { get; set; }

        [JsonIgnore]
        public virtual FacetGroup FacetGroup { get; set; }
        [JsonIgnore]
        public virtual FacetType FacetType { get; set; }
        [JsonIgnore]
        public virtual ICollection<FacetClause> Clauses { get; set; }
        [JsonIgnore]
        public virtual ICollection<FacetTable> Tables { get; set; }
    }
}
