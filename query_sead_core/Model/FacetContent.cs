using System;
using System.Collections.Generic;
using System.Linq;
using static QuerySeadDomain.Utility;

namespace QuerySeadDomain
{
    public class FacetContent {

        public class CategoryItem {
            public EFacetType FacetType { get; set;  }
            public string Category { get; internal set; }
            public string DisplayName { get; internal set; }
            public string Name { get; internal set; }
            public int Count { get; internal set; }
            public Dictionary<EFacetPickType, decimal> CategoryDetails { get; internal set; }
        }

        public FacetsConfig2 FacetsConfig { get; set; }
        public List<CategoryItem> Items { get; set; } = new List<CategoryItem>();
        public Dictionary<string, CategoryCountValue> FilteredDistribution { get; set; }
        public Dictionary<string, FacetsConfig2.UserPickData> PickMatrix { get; set; }

        public string FacetCode { get => FacetsConfig.TargetCode; }
        public string RequestType { get => FacetsConfig.RequestType; }
        public int ItemCount { get => Items.Count(); }
        public int PageOffset { get => FacetsConfig.TargetConfig.StartRow; }
        public int PageSize { get => FacetsConfig.TargetConfig.RowCount; }

        public int Interval { get; set; }
        public string IntervalQuery { get; set; }
        public int CountOfSelections { get; set; } = 0;

        public FacetContent(
            FacetsConfig2 facetsConfig,
            List<CategoryItem> items,
            Dictionary<string, CategoryCountValue> filteredCounts,
            Dictionary<string, FacetsConfig2.UserPickData> pickMatrix,
            int interval, string intervalQuery)
        {
            FacetsConfig         = facetsConfig;
            Items                = items;
            FilteredDistribution = filteredCounts;
            Interval             = interval;
            IntervalQuery        = intervalQuery;
            PickMatrix           = pickMatrix ?? new Dictionary<string, FacetsConfig2.UserPickData>();
        }

        public (int,int) getPage(int minSize=12)
        {
            if (FacetsConfig.TargetFacet.FacetTypeId == EFacetType.Range) {
                return (0, 250);
            }
            (int offset, int size) = FacetsConfig.TargetConfig.GetPage();
            if (RequestType == "populate_text_search") {
                offset = array_find_index<CategoryItem>(Items, FacetsConfig.TargetConfig.TextFilter, z => z.Name);
                offset = Math.Max(0, Math.Min(offset, ItemCount - minSize));
            }
            return (offset, size);
        }
    }
}