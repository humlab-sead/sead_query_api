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

    /// <summary>
    /// Facet definition ID and name
    /// </summary>
    public class FacetType {

        /// <summary>
        /// Facet type id
        /// </summary>
        public EFacetType FacetTypeId { get; set; }

        /// <summary>
        /// Type of facet in plain text
        /// </summary>
        public string FacetTypeName { get; set; }

        /// <summary>
        /// Specifies if facets of type should be reloaded when is target facet
        /// </summary>
        public bool ReloadAsTarget { get; set; }

    }

    /// <summary>
    /// Facet definition group type
    /// </summary>
    public class FacetGroup {

        [JsonIgnore]
        public int FacetGroupId { get; set; }

        public string FacetGroupKey { get; set; }
        public string DisplayTitle { get; set; }
        public bool IsApplicable { get; set; }
        public bool IsDefault { get; set; }

        public List<FacetDefinition> Items { get; set; }
    }

    /// <summary>
    /// Condition clauses associated to a facet
    /// </summary>
    public class FacetConditionClause {

        public int FacetSourceTableId { get; set; }
        public int FacetId { get; set; }
        public string Clause { get; set; }

        [JsonIgnore] public FacetDefinition FacetDefinition { get; set; }
    }

    /// <summary>
    /// A relational table associated to a facet
    /// </summary>
    public class FacetTable {

        public int FacetTableId { get; set; }
        public int FacetId { get; set; }
        public int SequenceId { get; set; }
        public string SchemaName { get; set; }
        public string TableName { get; set; }
        public string Alias { get; set; }

        [JsonIgnore]
        public FacetDefinition FacetDefinition { get; set; }
    }

    [JsonObject(MemberSerialization.OptOut)]
    public class FacetDefinition {

        /// <summary>
        /// System ID
        /// </summary>
        public int FacetId { get; set; }

        /// <summary>
        /// Domain key / ID
        /// </summary>
        public string FacetCode { get; set; }

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
        public FacetType FacetType { get; set; }

        [JsonIgnore]
        public FacetGroup FacetGroup { get; set; }

        [JsonIgnore]
        public List<FacetTable> Tables { get; set; }

        [JsonIgnore]
        public List<FacetConditionClause> Clauses { get; set; }

        [JsonIgnore]
        [NotMapped]
        public FacetTable TargetTable => Tables?.Find(z => z.SequenceId == 1) ?? null;

        [JsonIgnore]
        public string TargetTableName => TargetTable?.TableName ?? "";

        [JsonIgnore]
        [NotMapped]
        public List<FacetTable> ExtraTables
        {
            get { return Tables?.Where(z => z.SequenceId != 1)?.ToList(); }
        }

        [JsonIgnore]
        public string aliasName = null;

        [JsonIgnore]
        [NotMapped]
        public string AliasName {
            get {
                return aliasName ?? (aliasName = Tables?.FirstOrDefault(z => !z.Alias.Equals(""))?.Alias ?? "");
            }
        }

        [JsonIgnore]
        [NotMapped]
        public bool HasAliasName
        {
            get {
                return AliasName != "";
            }
        }

        [JsonIgnore]
        [NotMapped]
        public string ResolvedName  => AliasName != "" ? AliasName : TargetTableName;

        [JsonIgnore]
        public string QueryCriteria => String.Join(" AND ", Clauses.Select(x => x.Clause));

        /// <summary>
        /// Checks if facet is affected by target "facet" given facet sequence defined by list of facet codes
        /// </summary>
        /// <param name="targetFacet"></param>
        /// <param name="facetCodes">Facet chain</param>
        /// <returns></returns>
        public bool IsAffectedBy(List<string> facetCodes, FacetDefinition targetFacet)
        {
            if (targetFacet.FacetCode == FacetCode)
                return targetFacet.FacetType.ReloadAsTarget;
            return facetCodes.IndexOf(targetFacet.FacetCode) > facetCodes.IndexOf(FacetCode);
        }
    }

    public class ViewState {
        public string Key { get; set; }
        public string Data { get; set; }
    }
}
