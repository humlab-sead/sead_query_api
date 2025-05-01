using SQT.Infrastructure;
using SQT.Scaffolding;
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
            base(new JsonFacetContextDataFixture(SeadJsonDataFolder()))
        {
        }
    }
}
