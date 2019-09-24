using System.Collections.Generic;

namespace SeadQueryCore
{
    public interface ICategoryBoundsService  {
        List<Key2Value<int, float>> Load();
    }
}
