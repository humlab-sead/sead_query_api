using SeadQueryCore.Model;

namespace SeadQueryCore.Services.Result
{
    public interface IResultProjectionHandoffBuilder
    {
        ResultProjectionHandoff Build(FacetsConfig2 facetsConfig, ResultConfig resultConfig);
    }
}
