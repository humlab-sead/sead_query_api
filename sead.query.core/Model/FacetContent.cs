using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using static SeadQueryCore.Utility;

namespace SeadQueryCore
{
    public class FacetContent {

        public class IntervalQueryInfo
        {
            [JsonIgnore]
            public int Count { get; set; } = 0;
            [JsonIgnore]
            public string Query { get; set; } = "";
        }

        /// <summary>
        /// Facet configuration used in request
        /// </summary>
        public FacetsConfig2 FacetsConfig { get; set; }

        /// <summary>
        /// List of items loaded for facet, one for each category in the filtered data
        /// </summary>
        public List<CategoryCountItem> Items { get; set; } = new List<CategoryCountItem>();

        /// <summary>
        /// Distribution (category counts) of result set filtered by facet configurationa and user picks
        /// </summary>
        public Dictionary<string, CategoryCountItem> Distribution { get; set; }

        /// <summary>
        /// List of user picks, including bogus-picks filtered out by a preceeding facet
        /// </summary>
        public Dictionary<string, FacetsConfig2.UserPickData> Picks { get; set; }

        [JsonIgnore]
        public string FacetCode { get => FacetsConfig.TargetCode; }

        [JsonIgnore]
        public string FacetTypeKey { get => FacetsConfig.TargetFacet.FacetTypeKey; }

        [JsonIgnore]
        public string RequestType { get => FacetsConfig.RequestType; }

        [JsonIgnore]
        public int ItemCount { get => Items.Count; }

        public IntervalQueryInfo IntervalInfo { get; set; }

        public int CountOfSelections { get; set; } = 0;

        [JsonConstructor]
        public FacetContent()
        {

        }

        public FacetContent(
            FacetsConfig2 facetsConfig,
            List<CategoryCountItem> contentCategoryCounts,
            Dictionary<string, CategoryCountItem> categoryCounts,
            Dictionary<string, FacetsConfig2.UserPickData> picks,
            IntervalQueryInfo intervalInfo
        )
        {
            FacetsConfig = facetsConfig;
            Items = contentCategoryCounts.Where(z => z.Count != null).ToList();
            Distribution = categoryCounts;
            IntervalInfo = intervalInfo;
            Picks = picks ?? new Dictionary<string, FacetsConfig2.UserPickData>();
        }
    }
}