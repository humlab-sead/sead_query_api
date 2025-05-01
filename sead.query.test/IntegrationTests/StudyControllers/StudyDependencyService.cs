using SQT.Infrastructure;
using SQT.Scaffolding;

namespace IntegrationTests.StudyDb
{
    public class StudyDependencyService : DependencyService
    {

        public StudyDependencyService() :
            base(new JsonFacetContextDataFixture(ScaffoldUtility.GetDataFolder("Json")))
        {
        }
    }
}
