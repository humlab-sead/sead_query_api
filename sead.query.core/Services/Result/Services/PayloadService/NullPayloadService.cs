namespace SeadQueryCore.Services.Result
{
    public class NullPayloadService : IResultPayloadService
    {
        public dynamic GetExtraPayload(FacetsConfig2 facetsConfig, string resultFacetCode)
        {
            return null;
        }
    }
}
