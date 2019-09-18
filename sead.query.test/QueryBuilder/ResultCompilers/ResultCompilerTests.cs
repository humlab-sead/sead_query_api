using Autofac.Features.Indexed;
using Moq;
using SeadQueryCore;
using SeadQueryCore.Model;
using SeadQueryCore.QueryBuilder;
using System;
using Xunit;

namespace SeadQueryTest.QueryBuilder.ResultCompilers
{
    public class ResultCompilerTests : IDisposable
    {
        private MockRepository mockRepository;

        private Mock<IQueryBuilderSetting> mockQueryBuilderSetting;
        private Mock<IRepositoryRegistry> mockRepositoryRegistry;
        private Mock<IQuerySetupBuilder> mockQuerySetupBuilder;
        private Mock<IIndex<string, IResultSqlQueryCompiler>> mockIndex;

        public ResultCompilerTests()
        {
            this.mockRepository = new MockRepository(MockBehavior.Strict);

            this.mockQueryBuilderSetting = this.mockRepository.Create<IQueryBuilderSetting>();
            this.mockRepositoryRegistry = this.mockRepository.Create<IRepositoryRegistry>();
            this.mockQuerySetupBuilder = this.mockRepository.Create<IQuerySetupBuilder>();
            this.mockIndex = this.mockRepository.Create<IIndex<string, IResultSqlQueryCompiler>>();
        }

        public void Dispose()
        {
            this.mockRepository.VerifyAll();
        }

        private ResultCompiler CreateResultCompiler()
        {
            return new ResultCompiler(
                this.mockQueryBuilderSetting.Object,
                this.mockRepositoryRegistry.Object,
                this.mockQuerySetupBuilder.Object,
                this.mockIndex.Object);
        }

        [Fact]
        public void Compile_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var resultCompiler = this.CreateResultCompiler();
            FacetsConfig2 facetsConfig = null;
            ResultConfig resultConfig = null;
            string facetCode = null;

            // Act
            var result = resultCompiler.Compile(
                facetsConfig,
                resultConfig,
                facetCode);

            // Assert
            Assert.True(false);
        }
    }
}
