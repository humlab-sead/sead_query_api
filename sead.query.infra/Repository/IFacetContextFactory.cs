using SeadQueryCore;

namespace SeadQueryInfra
{
    public interface IFacetContextFactory
    {
        IFacetContext GetInstance();
    }
}
