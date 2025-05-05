using SeadQueryInfra;
using SQT.Infrastructure;
using SQT.Mocks;
using SQT.Scaffolding;
using System.IO;

namespace IntegrationTests.Sead
{
    public class FacetDependencyService : DependencyService
    {
        public FacetDependencyService() :
            base(new JsonSeededFacetContextFactory().Create("Json"))
        {
        }
    }
}
