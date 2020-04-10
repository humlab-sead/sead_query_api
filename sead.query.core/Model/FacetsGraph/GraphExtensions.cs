using System.Collections.Generic;
using System.Linq;

namespace SeadQueryCore
{
    public static class GraphExtensions
    {
        public static IEnumerable<TableRelation> ReverseEdges(this IEnumerable<TableRelation> edges)
        {
            return edges
                .Where(z => z.SourceTableId != z.TargetTableId)
                .Select(x => x.Reverse())
                .Where(z => !edges.Any(w => w.Equals(z)));
        }

        public static Dictionary<int, Dictionary<int, int>> ToWeights(this IEnumerable<TableRelation> edges) => edges
                .GroupBy(p => p.SourceTableId, (key, g) => (SourceId: key, TargetWeights: g.ToDictionary(x => x.TargetTableId, x => x.Weight)))
                .ToDictionary(x => x.SourceId, y => y.TargetWeights);

    }
}