using SQT.Infrastructure;
using SQT.Mocks;
using SQT.Scaffolding;

namespace Deprecated.StudyDb
{
    public class StudyDependencyService : DependencyService
    {

        public StudyDependencyService() :
            base(new InMemoryFacetContextFactory().Create(ScaffoldUtility.GetInMemoryDataFolder("Data/StudyDb")))
        {
        }
    }
}
