using Autofac;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuerySeadDomain;
using QuerySeadTests.fixtures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Dynamic;
using Newtonsoft.Json;

namespace QuerySeadTests
{
    [TestClass]
    public class ResultContentServiceTests
    {
        private FacetConfigFixture facetConfigFixture;
        private ResultConfigFixture resultConfigFixture;

        private TestContext testContextInstance;
        public TestContext TestContext
        {
            get { return testContextInstance; }
            set { testContextInstance = value; }
        }

        [TestInitialize()]
        public void Initialize()
        {
            facetConfigFixture = new fixtures.FacetConfigFixture();
            resultConfigFixture = new fixtures.ResultConfigFixture();
        }

        [TestMethod]
        public void LoadOfFinishSitesShouldEqualExpectedItems()
        {
            // Arrange
            IContainer container = new TestDependencyService().Register();
            var facetsConfig = facetConfigFixture.GenerateByUri("sites@sites:country@73/sites:");
            var resultConfig = resultConfigFixture.GenerateConfig("tabular", "site_level");
            using (var scope = container.BeginLifetimeScope())
            {
                var context = scope.Resolve<IUnitOfWork>();
                facetsConfig.SetContext(context);

                var service = scope.ResolveKeyed<IResultService>("tabular");
                var definition = context.Results.GetByKey(resultConfig.AggregateKeys[0]);
                // Act
                var resultSet = service.Load(facetsConfig, resultConfig);

                // Assert
                Assert.IsNotNull(resultSet);
                var expectedFields = definition.GetResultFields();
                var items = resultSet.Data.DataCollection.ToList();
                Assert.AreEqual(expectedFields.Count, items[0].Length, "Item count unexpected");
                Assert.IsTrue(items.All(x => x.Length == expectedFields.Count), "Item count unexpected");
                Assert.AreEqual(35, items.Count);

                var columns = resultSet.Meta.Columns;

                Assert.AreEqual(expectedFields.Count, columns.Count);
            }

        }
    }
}

