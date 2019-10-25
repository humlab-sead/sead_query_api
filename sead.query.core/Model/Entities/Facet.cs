using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace SeadQueryCore
{

    /// <summary>
    /// Enum for supported facet types
    /// </summary>
    public enum EFacetType {
        Unknown = 0,
        Discrete = 1,
        Range = 2,
        Geo = 3
    }

    [JsonObject(MemberSerialization.OptOut)]
    public class Facet {
        public Facet()
        {
            Clauses = new List<FacetClause>();
            Tables = new List<FacetTable>();
        }

        /// <summary>
        /// System ID
        /// </summary>
        public int FacetId { get; set; }

        /// <summary>
        /// Domain key / ID
        /// </summary>
        public virtual string FacetCode { get; set; }

        /// <summary>
        /// Display text
        /// </summary>
        public string DisplayTitle { get; set; }

        /// <summary>
        /// Group facet belongs to
        /// </summary>
        [JsonIgnore]
        public int FacetGroupId { get; set; }

        public string FacetGroupKey { get { return FacetGroup?.FacetGroupKey ?? "unknown";  } }

        /// <summary>
        /// Facet type ID
        /// </summary>
        [JsonIgnore]
        public EFacetType FacetTypeId { get; set; }

        public string FacetTypeKey { get { return FacetType?.FacetTypeName ?? "unknown"; } }

        /// <summary>
        /// Query (SQL) expression that specifies ID for facet category values
        /// </summary>
        [JsonIgnore]
        public string CategoryIdExpr { get; set; }

        /// <summary>
        /// Query (SQL) expression that specifies descriptive name facet category values
        /// </summary>
        [JsonIgnore]
        public string CategoryNameExpr { get; set; }

        [JsonIgnore]
        public string IconIdExpr { get; set; }

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

        [JsonIgnore]
        public virtual FacetGroup FacetGroup { get; set; }

        [JsonIgnore]
        public virtual List<FacetTable> Tables { get; set; }

        [JsonIgnore]
        public virtual List<FacetClause> Clauses { get; set; }

        [JsonIgnore]
        [NotMapped]
        public FacetTable TargetTable => Tables?.Find(z => z.SequenceId == 1) ?? null;

        // [JsonIgnore]
        // [NotMapped]
        // public List<FacetTable> ExtraTables
        // {
        //     get { return Tables?.Where(z => z.SequenceId != 1)?.ToList(); }
        // }

        [JsonIgnore]
        public string QueryCriteria => String.Join(" AND ", Clauses.Select(x => x.Clause));

        /// <summary>
        /// Checks if facet is affected by target "facet" given facet sequence defined by list of facet codes
        /// </summary>
        /// <param name="targetFacet"></param>
        /// <param name="facetCodes">Facet chain</param>
        /// <returns></returns>
        public bool IsAffectedBy(List<string> facetCodes, Facet targetFacet)
        {
            if (targetFacet.FacetCode == FacetCode)
                return targetFacet.FacetType.ReloadAsTarget;
            return facetCodes.IndexOf(targetFacet.FacetCode) > facetCodes.IndexOf(FacetCode);
        }
    }
}
