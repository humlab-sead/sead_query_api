using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using static SeadQueryCore.Utility;

namespace SeadQueryCore
{
    public class FacetContent {
        public class ContentItem {
            public string Category { get; set; }
            public string DisplayName { get; set; }
            public string Name { get; set; }
            public int? Count { get; set; }
            public List<decimal> Extent { get; set; }
        }

        /// <summary>
        /// Facet configuration used in request
        /// </summary>
        public FacetsConfig2 FacetsConfig { get; set; }

        /// <summary>
        /// List of items loaded for facet, one for each category in the filtered data
        /// </summary>
        public List<ContentItem> Items { get; set; } = new List<ContentItem>();

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

        [JsonIgnore]
        public int Interval { get; set; }

        [JsonIgnore]
        public string IntervalQuery { get; set; }

        public int CountOfSelections { get; set; } = 0;

        public FacetContent(
            FacetsConfig2 facetsConfig,
            List<ContentItem> items,
            Dictionary<string, CategoryCountItem> filteredCounts,
            Dictionary<string, FacetsConfig2.UserPickData> picks,
            int interval, string intervalQuery)
        {
            FacetsConfig = facetsConfig;
            Items = items.Where(z => z.Count != null).ToList();
            Distribution = filteredCounts;
            Interval = interval;
            IntervalQuery = intervalQuery;
            Picks = picks ?? new Dictionary<string, FacetsConfig2.UserPickData>();
        }
    }
}