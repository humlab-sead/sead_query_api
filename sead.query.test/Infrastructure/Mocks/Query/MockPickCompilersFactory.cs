using Autofac.Features.Indexed;
using Moq;
using SeadQueryCore;
using SeadQueryCore.QueryBuilder;

namespace SQT.Mocks
{
    internal static class MockPickCompilersFactory
    {
        public static IIndex<int, IPickFilterCompiler> Create(string returnValue = "")
        {
            var mockPickCompiler = new Mock<IPickFilterCompiler>();

            mockPickCompiler.Setup(foo => foo.Compile(It.IsAny<Facet>(), It.IsAny<Facet>(), It.IsAny<FacetConfig2>()))
                .Returns(returnValue);

            return new MockIndex<int, IPickFilterCompiler>
            {
                    { 1, mockPickCompiler.Object },
                    { 2, mockPickCompiler.Object }
            };
        }
    }
}
