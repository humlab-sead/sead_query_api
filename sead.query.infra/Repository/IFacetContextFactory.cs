using SeadQueryCore;

namespace SeadQueryInfra
{
    public interface IFacetContextFactory
    {
        FacetContext GetInstance();
    }
}
