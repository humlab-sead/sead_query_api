using Moq;
using SeadQueryCore;
using SeadQueryInfra;
using SQT.Infrastructure;
using SQT.Mocks;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace SQT.Infrastructure.Repository
{
    [Collection("JsonSeededFacetContext")]
    public class ResultRepositoryTests : DisposableFacetContextContainer
    {
        public ResultRepositoryTests(JsonSeededFacetContextFixture fixture) : base(fixture)
        {
        }

        private ResultRepository MockResultRepository()
        {
            return new ResultRepository(FacetContext);
        }

        [Fact]
        public void ToDictionary_Called_Success()
        {
            // Arrange
            var resultRepository = MockResultRepository();

            // Act
            var result = resultRepository.ToDictionary();

            // Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result);
        }

        [Fact]
        public void GetAll_Called_Success()
        {
            // Arrange
            var resultRepository = MockResultRepository();

            // Act
            var result = resultRepository.GetAll();

            // Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result);
        }

        [Theory]
        [InlineData("site_level")]
        public void GetByKey_Called_Success(string key)
        {
            // Arrange
            var resultRepository = MockResultRepository();

            // Act
            var result = resultRepository.GetByKey(key);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(key, result.CompositeKey);
            Assert.NotEmpty(result.GetResultFields());
        }

        [Fact]
        public void GetAllFields_Called_Success()
        {
            // Arrange
            var resultRepository = MockResultRepository();

            // Act
            var result = resultRepository.GetAllFields();

            // Assert
            Assert.NotEmpty(result);
        }

        [Fact]
        public void GetViewTypes_Called_Success()
        {
            // Arrange
            var resultRepository = MockResultRepository();

            // Act
            var result = resultRepository.GetViewTypes();

            // Assert
            Assert.NotEmpty(result);
        }

        [Theory]
        [InlineData("tabular")]
        [InlineData("map")]
        public void GetViewType_Called_Success(string viewTypeId)
        {
            // Arrange
            var resultRepository = MockResultRepository();

            // Act
            var result = resultRepository.GetViewType(viewTypeId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(viewTypeId, result.ViewTypeId);
        }

        [Fact]
        public void GetAllFieldTypes_Called_Success()
        {
            // Arrange
            var resultRepository = MockResultRepository();

            // Act
            var result = resultRepository.GetAllFieldTypes();

            // Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result);
        }

        [Theory]
        [InlineData("ste_level")]
        public void GetByKeys_StateUnderTest_ExpectedBehavior(params string[] keys)
        {
            // Arrange
            var resultRepository = MockResultRepository();

            // Act
            var result = resultRepository.GetByKeys(keys.ToList());

            // Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result);
        }

        [Theory]
        [InlineData("site_level")]
        public void GetFieldsByKeys_StateUnderTest_ExpectedBehavior(params string[] keys)
        {
            // Arrange
            var resultRepository = MockResultRepository();

            // Act
            var result = resultRepository.GetFieldsByKeys(keys.ToList());

            // Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result);
        }

        [Fact]
        public void GetAllFields_Called_SameAsDbSet()
        {
            // Arrange
            var resultRepository = MockResultRepository();

            // Act
            var result = resultRepository.GetAllFields().ToList();

            // Assert
            var expected = FacetContext.Set<ResultField>().ToList();
            Assert.Equal(expected, result);
        }

        [Fact]
        public void GetAll_Called_CompleteComposite()
        {
            // Arrange
            var resultRepository = MockResultRepository();

            // Act
            var result = resultRepository.GetAll().ToList();

            // Assert
            Assert.NotEmpty(result);
            foreach (var value in result) {
                Assert.NotNull(value.Fields);
                Assert.NotEmpty(value.Fields);
                Assert.All(value.Fields, z => Assert.NotNull((z).ResultField));
                Assert.All(value.Fields, z => Assert.NotNull((z).FieldType));
            }
        }

        [Theory]
        [InlineData("single_item")]
        [InlineData("text_agg_item")]
        [InlineData("count_item")]
        [InlineData("link_item")]
        [InlineData("sort_item")]
        [InlineData("link_item_filtered")]
        public void GetAllFieldTypes_Called_ExpectedTypes(string expectedType)
        {
            // Arrange
            var resultRepository = MockResultRepository();

            // Act
            var result = resultRepository.GetAllFieldTypes().ToList();

            // Assert
            Assert.NotEmpty(result);
            Assert.Single(result.Where(z => z.FieldTypeId == expectedType));
        }
    }
}
