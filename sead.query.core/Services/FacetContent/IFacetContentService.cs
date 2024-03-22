using System.Collections.Generic;

namespace SeadQueryCore
{
    public interface IFacetContentService
    {
        FacetContent Load(FacetsConfig2 facetsConfig);
    }
}