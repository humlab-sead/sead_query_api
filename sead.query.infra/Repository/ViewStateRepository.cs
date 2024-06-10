using SeadQueryCore;

namespace SeadQueryInfra
{
    public class ViewStateRepository(IRepositoryRegistry registry) : Repository<ViewState, string>(registry), IViewStateRepository
    {
    }
}
