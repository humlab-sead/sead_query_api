using SQT.Infrastructure;
using System.IO;

namespace IntegrationTests.StudyDb
{
    public class StudyDependencyService : DependencyService
    {
        public static string JsonDataFolder()
        {
            return Path.Combine(ScaffoldUtility.GetRootFolder(), "Infrastructure", "Data", "StudyModel", "Json");
        }

        public StudyDependencyService() :
            base(new JsonFacetContextFixture(JsonDataFolder()))
        {
        }
    }
}
