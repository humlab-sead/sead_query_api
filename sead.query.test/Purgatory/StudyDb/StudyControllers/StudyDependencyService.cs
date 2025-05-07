using SQT.Infrastructure;
using SQT.Mocks;

namespace Deprecated.StudyDb
{
    public class StudyDependencyService : DependencyService
    {

        public StudyDependencyService() :
            base(new JsonSeededFacetContextFactory().Create("Json"))
        {
        }
    }
}
