using Newtonsoft.Json;
using System.Collections.Generic;

namespace SeadQueryCore
{
    /// <summary>
    /// Gives the number of occurrences (Count) of a category determined by given extent
    /// The extent is a single value for discrete facets (the category ID) and an interval (lower, upper) for range facets
    /// </summary>
    public class CategoryCountItem {
        public string Category { get; set; }
        public int? Count { get; set; }
        public List<decimal> Extent;

        [JsonIgnore]
        public decimal Lower
        {
            get { return GetExtent(0); }
            set { SetExtent(0, value);  }
        }

        [JsonIgnore]
        public decimal Upper
        {
            get { return GetExtent(1); }
            set { SetExtent(1, value); }
        }

        private decimal GetExtent(int index)
        {
            if ((Extent?.Count ?? 0) > index)
                return Extent[index];
            return 0;
        }

        private void SetExtent(int index, decimal value)
        {
            if (Extent == null)
                Extent = new List<decimal>() { 0, 0 };
            Extent[index] = value;
        }
    }
}
