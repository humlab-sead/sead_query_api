using System.Collections.Generic;

namespace SeadQueryCore.Plugin.Intersect;


public class IntersectCategoryCountHelper(IFacetSetting config) : Range.RangeCategoryCountHelper(config), IIntersectCategoryCountHelper
{
}
