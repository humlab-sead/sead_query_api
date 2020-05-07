using Newtonsoft.Json.Linq;
using SeadQueryCore.Model;

namespace SeadQueryAPI.Serializers
{
    public interface IResultConfigReconstituteService
    {
        ResultConfig Reconstitute(JObject resultConfigJson);
        ResultConfig Reconstitute(ResultConfig resultConfig);
    }
}