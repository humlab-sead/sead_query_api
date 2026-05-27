using System.Collections.Generic;
using System.Threading.Tasks;
using SeadQueryCore;

namespace SeadQueryComposer.QueryComposer.Services;

public interface IFacetContentService
{
    Task<FacetContent> LoadAsync(FacetsConfig2 facetsConfig);
}
