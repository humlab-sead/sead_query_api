using System;

namespace SeadQueryCore
{

    // FIXME Eliminate explicit EPickType - should be a configurable in case if new kinds of facets...
    public enum EPickType
    {
        unknown = 0,
        discrete = 1,
        lower = 2,
        upper = 3
    }
}
