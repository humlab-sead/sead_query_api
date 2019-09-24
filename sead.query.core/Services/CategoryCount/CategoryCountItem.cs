using System.Collections.Generic;

namespace SeadQueryCore
{
    //public interface ICategoryCountServiceAggregate
    //{
    //    RangeCategoryCountService RangeCategoryCountService { get; set; }
    //    DiscreteCategoryCountService DiscreteCategoryCountService { get; set; }
    //}

    /// <summary>
    /// Gives the number of occurrences (Count) of a category determined by given extent
    /// The extent is a single value for discrete facets (the category ID) and an interval (lower, upper) for range facets
    /// </summary>
    public class CategoryCountItem {
        public string Category { get; set; }
        public int? Count { get; set; }
        public List<decimal> Extent;
        //public Dictionary<EFacetPickType, decimal> CategoryValues;
    }

}
