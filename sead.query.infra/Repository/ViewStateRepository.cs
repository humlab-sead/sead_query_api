using SeadQueryCore;

namespace SeadQueryInfra
{

    public class ViewStateRepository : Repository<ViewState, string>, IViewStateRepository
    {
        public ViewStateRepository(IFacetContext context) : base(context)
        {
        }

    }

}
