using System.Collections.Generic;

namespace SeadQueryCore
{
    public interface ICategoryCountService {
        CategoryCountService.CategoryCountResult Load(string facetCode, FacetsConfig2 facetsConfig, string intervalQuery);
    }

    public interface IDiscreteCategoryCountService : ICategoryCountService { }
}
