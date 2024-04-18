using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SeadQueryCore
{
    public class FacetContent
    {
        public class CategoryInfo
        {
            [JsonIgnore]
            public int Count { get; set; } = 0;
            [JsonIgnore]
            public string Query { get; set; } = "";
            [JsonIgnore]
            public List<decimal> Extent { get; set; }

        }

        /// <summary>
        /// Facet configuration used in request
        /// </summary>
        public FacetsConfig2 FacetsConfig { get; set; }

        /// <summary>
        /// List of items loaded for facet, one for each category in the filtered data
        /// </summary>
        public List<CategoryItem> Items { get; set; } = new List<CategoryItem>();

        /// <summary>
        /// Distribution (category counts) of result set filtered by facet configurationa and user picks
        /// </summary>
        public Dictionary<string, CategoryItem> Distribution { get; set; }

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

        public string SqlQuery { get; set; }

        public CategoryInfo IntervalInfo { get; set; }

        public int CountOfSelections { get; set; } = 0;

        [JsonConstructor]
        public FacetContent()
        {
        }

        public FacetContent(
            FacetsConfig2 facetsConfig,
            List<CategoryItem> contentCategoryCounts,
            Dictionary<string, CategoryItem> categoryCounts,
            string categoryCountSqlQuery,
            Dictionary<string, FacetsConfig2.UserPickData> picks,
            CategoryInfo intervalInfo
        )
        {
            FacetsConfig = facetsConfig;
            Items = contentCategoryCounts.Where(z => z.Count != null).ToList();
            Distribution = categoryCounts;
            IntervalInfo = intervalInfo;
            SqlQuery = categoryCountSqlQuery;
            Picks = picks ?? [];
        }
    }
}
