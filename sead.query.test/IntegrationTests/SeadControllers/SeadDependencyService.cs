using SQT.Infrastructure;
using System.IO;

namespace IntegrationTests.Sead
{
    public class FacetDependencyService : DependencyService
    {
        public static string SeadJsonDataFolder()
        {
            return Path.Combine(ScaffoldUtility.GetRootFolder(), "Infrastructure", "Data", "Json");
        }

        public FacetDependencyService() :
            base(new JsonFacetContextFixture(SeadJsonDataFolder()))
        {
        }
    }
}
