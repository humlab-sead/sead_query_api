using Autofac;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SeadQueryTest.Infrastructure;

namespace SeadQueryTest2.FacetsConfig
{
    [TestClass]
    public class QueryBuilderTests
    {
        private SeadQueryTest.fixtures.FacetConfigGenerator fixture;
        private static IContainer container;
        private TestContext testContextInstance;

        /// <summary>
        ///  Gets or sets the test context which provides
        ///  information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get { return testContextInstance; }
            set { testContextInstance = value; }
        }

        [ClassInitialize()]
        public static void InitializeClass(TestContext context)
        {
            container = new TestDependencyService().Register();
        }

        [TestInitialize()]
        public void Initialize() {
            fixture = new SeadQueryTest.fixtures.FacetConfigGenerator(null, null);
        }


    }
}
