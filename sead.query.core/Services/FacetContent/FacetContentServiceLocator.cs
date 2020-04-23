using Autofac.Features.Indexed;

namespace SeadQueryCore
{
    public class FacetContentServiceLocator : IFacetContentServiceLocator
    {
        public FacetContentServiceLocator(IIndex<EFacetType, IFacetContentService> services)
        {
            Services = services;
        }

        public IIndex<EFacetType, IFacetContentService> Services { get; }

        public virtual IFacetContentService Locate(EFacetType facetType)
        {
            return Services[facetType];
        }

    }
}
