using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace SeadQueryCore
{
    public enum EFacetType
    {
        Unknown = 0,
        Discrete = 1,
        Range = 2,
        GeoPolygon = 3,
        Intersect = 4
    }

    [JsonObject(MemberSerialization.OptOut)]
    public class Facet
    {
        public Facet()
        {
            Clauses = new List<FacetClause>();
            Tables = new List<FacetTable>();
            Children = new List<FacetChild>();
        }

        public int FacetId { get; set; }

        public virtual string FacetCode { get; set; }

        public string DisplayTitle { get; set; }
        public string Description { get; set; } = "";

        [JsonIgnore]
        public int FacetGroupId { get; set; }

        public string FacetGroupKey { get { return FacetGroup?.FacetGroupKey ?? "unknown"; } }

        [JsonIgnore]
        public EFacetType FacetTypeId { get; set; }

        public string FacetTypeKey { get { return FacetType?.FacetTypeName ?? "unknown"; } }

        [JsonIgnore]
        public string CategoryIdExpr { get; set; }

        [JsonIgnore]
        public string CategoryIdType { get; set; }

        [JsonIgnore]
        public string CategoryNameExpr { get; set; }

        public bool IsApplicable { get; set; }
        public bool IsDefault { get; set; }
        public string AggregateType { get; set; }
        public string AggregateTitle { get; set; }

        [JsonIgnore]
        public int AggregateFacetId { get; set; }

        [JsonIgnore]
        public string SortExpr { get; set; }

        [JsonIgnore]
        public virtual FacetType FacetType { get; set; }

        //[JsonIgnore]
        public virtual FacetGroup FacetGroup { get; set; }

        [JsonIgnore]
        public virtual List<FacetTable> Tables { get; set; }

        [JsonIgnore]
        public virtual List<FacetClause> Clauses { get; set; }

        [JsonIgnore]
        public virtual IEnumerable<FacetChild> Children { get; set; }

        [JsonIgnore]
        [NotMapped]
        public FacetTable TargetTable => Tables?.Find(z => z.SequenceId == 1) ?? null;

        [JsonIgnore]
        public string Criteria => String.Join(" AND ", Criterias);

        [JsonIgnore]
        public IEnumerable<string> Criterias => Clauses.Select(x => x.Clause);

        public IEnumerable<string> GetResolvedTableNames() =>
            Tables.Select(x => x.ResolvedAliasOrTableOrUdfName);

    }
}
