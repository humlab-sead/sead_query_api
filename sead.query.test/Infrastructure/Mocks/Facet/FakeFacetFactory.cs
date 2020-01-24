using Moq;
using SeadQueryCore;
using SeadQueryTest.Fixtures;

namespace SeadQueryTest.Mocks
{
    internal static class FacetFactory
    {
        public static Facet Create(string facetCode)
        {
            return FacetFixtures.Store[facetCode];
        }

    }
}
