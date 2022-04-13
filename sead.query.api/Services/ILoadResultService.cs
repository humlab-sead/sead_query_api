using Autofac.Features.Indexed;
using SeadQueryCore;
using SeadQueryCore.Model;
using SeadQueryCore.Services.Result;

namespace SeadQueryAPI.Services
{
    public interface ILoadResultService
    {
        ResultContentSet Load(FacetsConfig2 facetsConfig, ResultConfig resultConfig);
    }

}