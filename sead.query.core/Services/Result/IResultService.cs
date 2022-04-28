
using SeadQueryCore.Model;

namespace SeadQueryCore.Services.Result
{
    public interface IResultService
    {
        ResultContentSet Load(FacetsConfig2 facetsConfig, ResultConfig resultConfig);
    }
}