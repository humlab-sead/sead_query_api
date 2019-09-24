using System.Collections.Generic;

namespace SeadQueryCore
{
    public interface ICategoryCountService {
        Dictionary<string, CategoryCountItem> Load(string facetCode, FacetsConfig2 facetsConfig, string intervalQuery);
    }

}
