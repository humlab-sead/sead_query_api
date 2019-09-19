using System.Collections.Generic;
using System.Linq;
using SeadQueryTest.Infrastructure;

namespace SeadQueryTest.fixtures
{

    public class RouteGenerator
    {
        public List<string> Trail { get; set; }
        public List<string> Pairs { get { return ToPairs(Trail); } }
        public RouteGenerator(List<string> trail)
        {
            Trail = trail;
        }
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
