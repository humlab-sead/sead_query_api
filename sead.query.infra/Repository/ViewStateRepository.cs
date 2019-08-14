using DataAccessPostgreSqlProvider;
using SeadQueryCore;

namespace SeadQueryInfra
{

    public class ViewStateRepository : Repository<ViewState, string>, IViewStateRepository
    {
        public ViewStateRepository(DomainModelDbContext context) : base(context)
        {
        }

    }

}
