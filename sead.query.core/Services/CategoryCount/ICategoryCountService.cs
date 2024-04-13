using System.Collections.Generic;

namespace SeadQueryCore
{
    public interface ICategoryCountService
    {
        CategoryCountService.CategoryCountData Load(string facetCode, FacetsConfig2 facetsConfig, string intervalQuery, EFacetType facetTypeOverride = EFacetType.Unknown);
    }

    // public interface IDiscreteCategoryCountService : ICategoryCountService { }
}
