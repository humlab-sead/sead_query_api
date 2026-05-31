using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Newtonsoft.Json;

namespace SeadQueryCore
{
    /// <summary>
    /// Enumeration of supported facet types in the SEAD query system
    /// </summary>
    public enum EFacetType
    {
        /// <summary>
        /// Unknown or unspecified facet type
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// Discrete/categorical facet with specific selectable values (e.g., country, material type)
        /// Facet Predicate Query (FPQ) expects a set of discrete values for filtering
        /// </summary>
        Discrete = 1,

        /// <summary>
        /// Range facet with min/max values (e.g., age ranges, measurements)
        /// Facet Predicate Query (FPQ) expects a range of values for filtering
        /// </summary>
        Range = 2,

        /// <summary>
        /// Geographic polygon facet for spatial filtering
        /// Facet Predicate Query (FPQ) expects a set of polygon geometries for filtering
        /// </summary>
        GeoPolygon = 3,

        /// <summary>
        /// Intersect facet using CTE-based query composition
        /// Facet Predicate Query (FPQ) expects a a range of values for filtering
        /// </summary>
        Intersect = 4,
    }

    /// <summary>
    /// Represents a facet in the SEAD faceted browser system.
    /// A facet defines a filterable dimension of data (e.g., sites, periods, materials)
    /// and contains the metadata needed to generate filtering queries.
    /// </summary>
    [JsonObject(MemberSerialization.OptOut)]
    public class Facet
    {
        /// <summary>
        /// Initializes a new instance of the Facet class with empty collections
        /// </summary>
        public Facet()
        {
            Clauses = [];
            Tables = [];
            Children = [];
        }

        /// <summary>
        /// Unique identifier for this facet in the database
        /// </summary>
        public int FacetId { get; set; }

        /// <summary>
        /// Unique code/key identifying this facet (e.g., "site", "country", "period")
        /// </summary>
        public virtual string FacetCode { get; set; }

        /// <summary>
        /// Human-readable display title for the facet (e.g., "Archaeological Sites", "Time Periods")
        /// </summary>
        public string DisplayTitle { get; set; }

        /// <summary>
        /// Optional detailed description of what this facet represents
        /// </summary>
        public string Description { get; set; } = "";

        /// <summary>
        /// Foreign key reference to the facet group this facet belongs to (UI grouping)
        /// </summary>
        [JsonIgnore]
        public int FacetGroupId { get; set; }

        /// <summary>
        /// Shortcut that returns key/code of the facet group this facet belongs to
        /// TODO Consider removing this redundant property
        /// </summary>
        public string FacetGroupKey
        {
            get { return FacetGroup?.FacetGroupKey ?? "unknown"; }
        }

        /// <summary>
        /// The type of facet (Discrete, Range, GeoPolygon, etc.) determining filtering behavior
        /// </summary>
        [JsonIgnore]
        public EFacetType FacetTypeId { get; set; }

        /// <summary>
        /// Shortcut that returns the human-readable name of the facet type
        /// TODO Consider removing this redundant property
        /// </summary>
        public string FacetTypeKey
        {
            get { return FacetType?.FacetTypeName ?? "unknown"; }
        }

        /// <summary>
        /// SQL expression used in filtering
        /// Also used for extracting the category/value identifier from the source table
        /// (e.g., "site_id", "country_iso_code", "age_range")
        /// </summary>
        [JsonIgnore]
        public string CategoryIdExpr { get; set; }

        /// <summary>
        /// Data type of the category identifier (e.g., "integer", "varchar", "int4range")
        /// Used for proper SQL casting and operations
        /// </summary>
        [JsonIgnore]
        public string CategoryIdType { get; set; }

        /// <summary>
        /// SQL operator used for filtering operations
        /// </summary>
        [JsonIgnore]
        public string CategoryIdOperator { get; set; }

        /// <summary>
        /// SQL expression for extracting the human-readable category name/label
        /// (e.g., "site_name", "country_name", "period_name")
        /// </summary>
        [JsonIgnore]
        public string CategoryNameExpr { get; set; }

        /// <summary>
        /// Indicates whether this facet is currently applicable/available for filtering
        /// </summary>
        public bool IsApplicable { get; set; }

        /// <summary>
        /// Indicates whether this facet should be shown by default in the UI
        /// </summary>
        public bool IsDefault { get; set; }

        /// <summary>
        /// Type of aggregation used when this facet serves as an anchor (e.g., "count", "sum")
        /// </summary>
        public string AggregateType { get; set; }

        /// <summary>
        /// Human-readable title for the aggregation (e.g., "Number of Sites", "Total Samples")
        /// </summary>
        public string AggregateTitle { get; set; }

        /// <summary>
        /// Foreign key reference to another facet used for aggregation purposes
        /// </summary>
        [JsonIgnore]
        public int AggregateFacetId { get; set; }

        /// <summary>
        /// SQL expression used for sorting facet results (e.g., "site_name ASC", "date_range DESC")
        /// </summary>
        [JsonIgnore]
        public string SortExpr { get; set; }

        /// <summary>
        /// Navigation property to the FacetType entity defining this facet's behavior
        /// </summary>
        [JsonIgnore]
        public virtual FacetType FacetType { get; set; }

        /// <summary>
        /// Navigation property to the FacetGroup this facet belongs to for organizational purposes
        /// </summary>
        //[JsonIgnore]
        public virtual FacetGroup FacetGroup { get; set; }

        /// <summary>
        /// Collection of database tables involved in this facet's queries.
        /// In the old monolithic design, this defines the join path from source to targets.
        /// In the new design, this is primarily used for the source table (SequenceId == 1).
        /// </summary>
        /// <remarks>
        ///            NOTE! PENDING DEPRECATION
        /// </remarks>
        [JsonIgnore]
        public virtual List<FacetTable> Tables { get; set; }

        public List<FacetAnchor> FacetAnchors { get; set; }

        /// <summary>
        /// Collection of additional SQL clauses/constraints that apply to this facet
        /// (e.g., "WHERE is_active = true", "AND date_created > '2020-01-01'")
        /// </summary>
        /// <remarks>
        ///            NOTE! PENDING DEPRECATION
        /// </remarks>
        [JsonIgnore]
        public virtual List<FacetClause> Clauses { get; set; }

        /// <summary>
        /// Collection of child facets that are hierarchically related to this facet
        /// </summary>
        [JsonIgnore]
        public virtual List<FacetChild> Children { get; set; }

        /// <summary>
        /// Gets the primary source table for this facet (the table containing the facet's values).
        /// This is typically the table with SequenceId == 1 in the Tables collection.
        /// In new terminology: this is the "source table" where facet values originate.
        /// </summary>
        /// <remarks>
        ///            NOTE! PENDING DEPRECATION
        /// </remarks>
        [JsonIgnore]
        [NotMapped]
        public FacetTable TargetTable => Tables?.Find(z => z.SequenceId == 1) ?? null;

        /// <summary>
        /// Gets all facet clauses combined into a single AND-separated criteria string
        /// </summary>
        /// <remarks>
        ///            NOTE! PENDING DEPRECATION
        /// </remarks>
        [JsonIgnore]
        public string Criteria => String.Join(" AND ", Criterias);

        /// <summary>
        /// Gets the individual clause strings from the Clauses collection
        /// </summary>
        /// <remarks>
        ///            NOTE! PENDING DEPRECATION
        /// </remarks>
        [JsonIgnore]
        public IEnumerable<string> Criterias => Clauses.Select(x => x.Clause);

        /// <summary>
        /// Gets the resolved table names (with aliases where applicable) for all tables in this facet
        /// NOTE! PENDING DEPRECATION
        /// </summary>
        /// <remarks>
        ///            NOTE! PENDING DEPRECATION
        /// </remarks>
        /// <returns>Collection of resolved table names used in SQL generation</returns>
        public IEnumerable<string> GetResolvedTableNames() => Tables.Select(x => x.ResolvedAliasOrTableOrUdfName);
    }
}
