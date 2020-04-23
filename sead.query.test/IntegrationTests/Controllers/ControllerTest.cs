using SQT.Mocks;
using Xunit;

namespace IntegrationTests
{
    public class ControllerTest<TFixture> : IClassFixture<TFixture> where TFixture : class
    {
        protected readonly MockFacetsConfigFactory FacetsConfigMockFactory;

        public TFixture Fixture { get; }

        public ControllerTest(TFixture fixture)
        {
            FacetsConfigMockFactory = new MockFacetsConfigFactory(null);
            Fixture = fixture;
        }
    }
}
