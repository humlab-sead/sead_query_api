using Autofac.Features.Indexed;

namespace SeadQueryCore.Services.Result
{
    public class ResultPayloadServiceLocator : IResultPayloadServiceLocator
    {
        public ResultPayloadServiceLocator(IIndex<string, IResultPayloadService> services)
        {
            Services = services;
        }

        public IIndex<string, IResultPayloadService> Services { get; }

        public virtual IResultPayloadService Locate(string viewType)
        {
            return Services[viewType];
        }

    }
}
