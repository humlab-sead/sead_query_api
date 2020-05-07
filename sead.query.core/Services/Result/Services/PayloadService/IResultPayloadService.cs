namespace SeadQueryCore.Services.Result
{
    public interface IResultPayloadService
    {
        dynamic GetExtraPayload(FacetsConfig2 facetsConfig, string resultFacetCode);
    }
}