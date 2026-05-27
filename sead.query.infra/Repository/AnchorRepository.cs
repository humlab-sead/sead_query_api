using SeadQueryCore;

namespace SeadQueryInfra
{
    public class AnchorRepository(RepositoryRegistry registry) : Repository<Anchor, int>(registry), IAnchorRepository { }
}
