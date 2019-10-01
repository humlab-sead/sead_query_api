using Newtonsoft.Json.Linq;
using SeadQueryCore;

namespace SeadQueryAPI.Serializers
{
    public interface IFacetConfigReconstituteService
    {
        FacetsConfig2 Reconstitute(string json);
        FacetsConfig2 Reconstitute(JObject json);
        FacetsConfig2 Reconstitute(FacetsConfig2 facetsConfig);
        FacetConfig2 Reconstitute(FacetConfig2 facetConfig);
    }
}