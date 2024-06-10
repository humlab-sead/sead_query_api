using System.Collections.Generic;
using System.Linq;
using SQT.Infrastructure;

namespace SQT.Mocks
{
    public static class RouteFactory
    {
        public static List<string> ToPairs(List<string> trail)
        {
            return trail.Take(trail.Count - 1).Select((e, i) => e + "/" + trail[i + 1]).ToList();
        }

        public static List<string> ToPairs(params string[] trail)
        {
            return ToPairs(trail.ToList());
        }
    }
}
