using SeadQueryCore;

namespace SeadQueryInfra
{
    public class ViewStateRepository(IFacetContext context) : Repository<ViewState, string>(context), IViewStateRepository
    {
    }
}
