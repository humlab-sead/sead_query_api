using Moq;
using SeadQueryCore;
using SeadQueryInfra;
using SeadQueryTest.Infrastructure;
using SeadQueryTest.Mocks;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace SeadQueryTest.Repository
{
    public class ResultRepositoryTests : DisposableFacetContextContainer
    {
        public ResultRepositoryTests(JsonSeededFacetContextFixture fixture) : base(fixture)
        {
        }

        private ResultRepository CreateRepository()
        {
            return new ResultRepository(Context);
        }

        [Fact(Skip = "Not implemented")]
        public void ToDictionary_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var resultRepository = this.CreateRepository();

            // Act
            var result = resultRepository.ToDictionary();

            // Assert
            Assert.True(false);
        }

        [Fact(Skip = "Not implemented")]
        public void GetAll_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var resultRepository = this.CreateRepository();

            // Act
            var result = resultRepository.GetAll();

            // Assert
            Assert.True(false);
        }

        [Fact(Skip = "Not implemented")]
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

        [Fact(Skip = "Not implemented")]
        public void GetAllFields_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var resultRepository = this.CreateRepository();

            // Act
            var result = resultRepository.GetAllFields();

            // Assert
            Assert.True(false);
        }

        [Fact(Skip = "Not implemented")]
        public void GetViewTypes_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var resultRepository = this.CreateRepository();

            // Act
            var result = resultRepository.GetViewTypes();

            // Assert
            Assert.True(false);
        }

        [Fact(Skip = "Not implemented")]
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

        [Fact(Skip = "Not implemented")]
        public void GetAllFieldTypes_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var resultRepository = this.CreateRepository();

            // Act
            var result = resultRepository.GetAllFieldTypes();

            // Assert
            Assert.True(false);
        }

        [Fact(Skip = "Not implemented")]
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

        [Fact(Skip = "Not implemented")]
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

            var sut = new ResultRepository(Context);

            var expected = Context.Set<ResultField>().ToList().Count;

            Assert.Equal(expected, sut.GetAllFields().ToList().Count);
        }

        [Fact]
        public void ShouldBeAbleToFetchAllResultAggregates()
        {
            var sut = new ResultRepository(Context);
            List<ResultAggregate> items = sut.GetAll().ToList();

            Assert.Equal(4, items.Count);
            foreach (ResultAggregate value in items) {
                Assert.NotNull(value.Fields);
                Assert.True(value.Fields.Count > 0);
                value.Fields.ForEach(z => Assert.NotNull(z.ResultField));
                value.Fields.ForEach(z => Assert.NotNull(z.FieldType));
            }
        }

        [Fact]
        public void ShouldBeAbleToFetchAllResultTypes()
        {
            var repository = new ResultRepository(Context);

            List<ResultFieldType> fieldTypes = repository.GetAllFieldTypes().ToList();

            Assert.True(fieldTypes.Count > 0);

            var expected = new List<string>() { "single_item", "text_agg_item", "count_item", "link_item", "sort_item", "link_item_filtered" };

            Assert.True(expected.All(x => fieldTypes.Exists(w => w.FieldTypeId == x)));
        }
    }
}
