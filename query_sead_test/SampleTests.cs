using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using DataAccessPostgreSqlProvider;
using System.Linq;
using QuerySeadDomain;
using System.Diagnostics;

namespace QuerySeadTests
{

    [TestClass]
    public class SampleTests
    {

        [AssemblyInitialize()]
        public static void AssemblyInit(TestContext context)
        {
            Debug.WriteLine("AssemblyInit " + context.TestName);
        }

        [ClassInitialize()]
        public static void ClassInit(TestContext context)
        {
            Debug.WriteLine("ClassInit " + context.TestName);
        }

        [TestInitialize()]
        public void Initialize()
        {
            Debug.WriteLine("TestMethodInit");
        }

        [TestCleanup()]
        public void Cleanup()
        {
            Debug.WriteLine("TestMethodCleanup");
        }

        [ClassCleanup()]
        public static void ClassCleanup()
        {
            Debug.WriteLine("ClassCleanup");
        }

        [AssemblyCleanup()]
        public static void AssemblyCleanup()
        {
            Debug.WriteLine("AssemblyCleanup");
        }

        [TestMethod]
        public void TrueMustBeTrue()
        {
            Assert.IsTrue(true);
        }

        [DataTestMethod]
        [DataRow(false)]
        public void FalseMustAlwaysBeFalse(bool value)
        {
            Assert.IsFalse(value);
        }


    }

}
