using Autofac.Features.Indexed;

namespace SeadQueryCore
{
    public class CategoryCountServiceLocator : ICategoryCountServiceLocator
    {
        public CategoryCountServiceLocator(IIndex<EFacetType, ICategoryCountService> services)
        {
            Services = services;
        }

        public IIndex<EFacetType, ICategoryCountService> Services { get; }

        public virtual ICategoryCountService Locate(EFacetType facetType)
        {
            return Services[facetType];
        }

    }
}
