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
        private FacetConfigGenerator facetConfigFixture;
        private ResultConfigGenerator resultConfigFixture;

        private TestContext testContextInstance;
        public TestContext TestContext
        {
            get { return testContextInstance; }
            set { testContextInstance = value; }
        }

        [TestInitialize()]
        public void Initialize()
        {
            facetConfigFixture = new fixtures.FacetConfigGenerator();
            resultConfigFixture = new fixtures.ResultConfigGenerator();
        }

        [TestMethod]
        public void LoadOfFinishSitesShouldEqualExpectedItems()
        {
            var testConfigs = new List<(string, string, string, int)>()
            {
                ("tabular", "site_level", "sites@sites:country@73/sites:", 30 ),
                ("tabular", "aggregate_all", "sites@sites:country@73/sites:", 1 ),
                ("tabular", "sample_group_level", "sites@sites:country@73/sites:", 30 ),
                ("map", "map_result", "sites@sites:country@73/sites:", 32 )
            };
            foreach (var (viewTypeId, resultKey, uri, expectedCount) in testConfigs)
                ExecuteLoadContent(expectedCount, viewTypeId, resultKey, uri);
        }

        private void ExecuteLoadContent(int expectedCount, string viewTypeId, string resultKey, string uri)
        {

            // Arrange
            IContainer container = new TestDependencyService().Register();
            var facetsConfig = facetConfigFixture.GenerateByUri(uri);
            var resultConfig = resultConfigFixture.GenerateConfig(viewTypeId, resultKey);
            using (var scope = container.BeginLifetimeScope()) {
                var context = scope.Resolve<IUnitOfWork>();
                facetsConfig.SetContext(context);

                var service = scope.ResolveKeyed<IResultService>(viewTypeId);
                var aggregate = context.Results.GetByKey(resultConfig.AggregateKeys[0]);

                // Act
                var resultSet = service.Load(facetsConfig, resultConfig);

                // Assert
                Assert.IsNotNull(resultSet);
                var expectedFields = aggregate.GetResultFields();
                var items = resultSet.Data.DataCollection.ToList();
                Assert.IsTrue(items.All(x => x.Length == expectedFields.Count), $"Field count unexpected. {viewTypeId}/{resultKey}");
                Assert.AreEqual(expectedCount, items.Count, $"Item count unexpected. {viewTypeId}/{resultKey}");

                var columns = resultSet.Meta.Columns;

                Assert.AreEqual(expectedFields.Count, columns.Count);
            }
        }
    }
}

