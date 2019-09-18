using Moq;
using SeadQueryCore;
using SeadQueryInfra;
using SeadQueryTest.Infrastructure.Scaffolding;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace SeadQueryTest.Repository
{
    public class ResultRepositoryTests : IDisposable
    {
        private IFacetContext mockFacetContext;

        public ResultRepositoryTests()
        {
            this.mockFacetContext = ScaffoldUtility.DefaultFacetContext();
        }

        public void Dispose()
        {
        }

        private ResultRepository CreateRepository()
        {
            return new ResultRepository(this.mockFacetContext);
        }

        [Fact]
        public void ToDictionary_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var resultRepository = this.CreateRepository();

            // Act
            var result = resultRepository.ToDictionary();

            // Assert
            Assert.True(false);
        }

        [Fact]
        public void GetAll_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var resultRepository = this.CreateRepository();

            // Act
            var result = resultRepository.GetAll();

            // Assert
            Assert.True(false);
        }

        [Fact]
        public void GetByKey_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var resultRepository = this.CreateRepository();
            string key = null;

            // Act
            var result = resultRepository.GetByKey(
                key);

            // Assert
            Assert.True(false);
        }

        [Fact]
        public void GetAllFields_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var resultRepository = this.CreateRepository();

            // Act
            var result = resultRepository.GetAllFields();

            // Assert
            Assert.True(false);
        }

        [Fact]
        public void GetViewTypes_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var resultRepository = this.CreateRepository();

            // Act
            var result = resultRepository.GetViewTypes();

            // Assert
            Assert.True(false);
        }

        [Fact]
        public void GetViewType_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var resultRepository = this.CreateRepository();
            string viewTypeId = null;

            // Act
            var result = resultRepository.GetViewType(
                viewTypeId);

            // Assert
            Assert.True(false);
        }

        [Fact]
        public void GetAllFieldTypes_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var resultRepository = this.CreateRepository();

            // Act
            var result = resultRepository.GetAllFieldTypes();

            // Assert
            Assert.True(false);
        }

        [Fact]
        public void GetByKeys_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var resultRepository = this.CreateRepository();
            List<string> keys = null;

            // Act
            var result = resultRepository.GetByKeys(
                keys);

            // Assert
            Assert.True(false);
        }

        [Fact]
        public void GetFieldsByKeys_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var resultRepository = this.CreateRepository();
            List<string> keys = null;

            // Act
            var result = resultRepository.GetFieldsByKeys(
                keys);

            // Assert
            Assert.True(false);
        }

        [Fact]
        public void ShouldBeAbleToFetchAllResultFields()
        {
            // Arrange
            using (var context = ScaffoldUtility.DefaultFacetContext()) {

                var sut = new ResultRepository(context);

                var expected = context.Set<ResultField>().ToList().Count;

                Assert.Equal(expected, sut.GetAllFields().ToList().Count);
            }
        }

        [Fact]
        public void ShouldBeAbleToFetchAllResultAggregates()
        {
            using (var context = ScaffoldUtility.DefaultFacetContext()) {

                var sut = new ResultRepository(context);
                List<ResultAggregate> items = sut.GetAll().ToList();

                Assert.Equal(4, items.Count);
                foreach (ResultAggregate value in items) {
                    Assert.NotNull(value.Fields);
                    Assert.True(value.Fields.Count > 0);
                    value.Fields.ForEach(z => Assert.NotNull(z.ResultField));
                    value.Fields.ForEach(z => Assert.NotNull(z.FieldType));
                }
            }
        }

        [Fact]
        public void ShouldBeAbleToFetchAllResultTypes()
        {
            using (var context = ScaffoldUtility.DefaultFacetContext()) {

                var repository = new ResultRepository(context);

                List<ResultFieldType> fieldTypes = repository.GetAllFieldTypes().ToList();

                Assert.True(fieldTypes.Count > 0);

                var expected = new List<string>() { "single_item", "text_agg_item", "count_item", "link_item", "sort_item", "link_item_filtered" };

                Assert.True(expected.All(x => fieldTypes.Exists(w => w.FieldTypeId == x)));
            }
        }
    }
}
