namespace SeadQueryCore.Services.Result
{
    public interface IResultPayloadServiceLocator
    {
        IResultPayloadService Locate(string viewType);
    }
}