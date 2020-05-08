using Newtonsoft.Json.Linq;
using SeadQueryAPI.DTO;
using SeadQueryCore.Model;

namespace SeadQueryAPI.Serializers
{
    public interface IResultConfigReconstituteService
    {
        ResultConfig Reconstitute(JObject resultConfigJson);
        ResultConfig Reconstitute(ResultConfigDTO resultConfig);
    }
}