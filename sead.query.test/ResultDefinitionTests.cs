using Autofac;
using DataAccessPostgreSqlProvider;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SeadQueryCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace SeadQueryTest
{

    [TestClass]
    public class ResultDefinitionTests
    {
        DomainModelDbContext context;
        IContainer container;
        IQueryBuilderSetting settings;

        public ResultDefinitionTests()
        {
            container = new TestDependencyService().Register();
            settings = container.Resolve<IQueryBuilderSetting>();
        }

        [TestInitialize()]
        public void Initialize()
        {
            Debug.WriteLine("Called: TestInitialize");
            context = new DomainModelDbContext(settings);
        }

        [TestCleanup()]
        public void Cleanup()
        {
            Debug.WriteLine("Called: TestCleanup");
            context.Dispose();
        }

        [TestMethod]
        public void ShouldBeAbleToFetchAllResultDefinitions()
        {
            var repository = new ResultRepository(context);

            List<ResultAggregate> resultDefinitions = repository.GetAll().ToList();

            Assert.AreEqual(4, resultDefinitions.Count);
            foreach (ResultAggregate value in resultDefinitions) {
                Assert.IsNotNull(value.Fields);
                Assert.IsTrue(value.Fields.Count > 0);
                value.Fields.ForEach(z => Assert.IsNotNull(z.ResultField));
                value.Fields.ForEach(z => Assert.IsNotNull(z.FieldType));
            }
        }

        [TestMethod]
        public void ShouldBeAbleToFetchAllResultTypes()
        {
            var repository = new ResultRepository(context);
            List<ResultFieldType> fieldTypes = repository.GetAllFieldTypes().ToList();
            Assert.IsTrue(fieldTypes.Count > 0);
            var expected = new List<string>() { "single_item", "text_agg_item", "count_item", "link_item", "sort_item", "link_item_filtered" };
            Assert.IsTrue(expected.All(x => fieldTypes.Exists(w => w.FieldTypeId == x)));
        }

        [TestMethod]
        public void ShouldBeAbleToFetchAllResultFieldTypesByContext()
        {
            var values = context.Set<ResultFieldType>().ToList();
            Assert.IsTrue(values.Count > 0);
        }

        [TestMethod]
        public void ShouldBeAbleToFetchAllResultFieldsByContext()
        {
            List<ResultField> values = context.Set<ResultField>().ToList();
            Assert.IsTrue(values.Count > 0);
        }

        [TestMethod]
        public void ShouldBeAbleToFetchAllResultFieldsByRepository()
        {
            var repository = new ResultRepository(context);
            List<ResultField> values = repository.GetAllFields().ToList();
            Assert.IsTrue(values.Count > 0);
            var expected = new List<string>() { "sitename","record_type","analysis_entities","site_link","site_link_filtered","aggregate_all_filtered","sample_group_link","sample_group_link_filtered","abundance","taxon_id","dataset","dataset_link","dataset_link_filtered","sample_group","methods" };
            Assert.IsTrue(expected.All(x => values.Exists(w => w.ResultFieldKey == x)));
        }

        [TestMethod]
        public void ShouldBeAbleToSerializeSequenceOfPrimitives()
        {
            var data  = new object[8] { 1, 1.2, true, null, "roger", 'a', 0, 1.23 };
            var x = JsonConvert.SerializeObject(data);
            Assert.IsNotNull(x);
        }

    }
}
