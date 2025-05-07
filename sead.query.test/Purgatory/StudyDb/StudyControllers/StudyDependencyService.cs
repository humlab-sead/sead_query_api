using SQT.Infrastructure;
using SQT.Mocks;
using SQT.Scaffolding;

namespace IntegrationTests.StudyDb
{
    public class StudyDependencyService : DependencyService
    {

        public StudyDependencyService() :
            base(new JsonSeededFacetContextFactory().Create("Json"))
        {
        }
    }
}
