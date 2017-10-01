using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using DataAccessPostgreSqlProvider;
using System.Linq;
using QuerySeadDomain;
using System.Diagnostics;
using Autofac;

namespace QuerySeadTests
{
    [TestClass]
    public class BaseTest
    {

        protected fixtures.FacetConfigFixture fixture;
        protected static IContainer container;
        protected string logDir = @"\temp\json\";

        protected TestContext testContextInstance;

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
        public void Initialize()
        {
            fixture = new fixtures.FacetConfigFixture();
        }

    }

}
