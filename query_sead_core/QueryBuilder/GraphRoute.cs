using System;
using System.Collections.Generic;
using System.Linq;

namespace QuerySeadDomain.QueryBuilder
{
    public class GraphRoute {

        public struct GraphRouteItem {
            public string SourceName { get; set; }
            public string TargetName { get; set; }
        }

        public List<GraphRouteItem> Items { get; set; } = new List<GraphRouteItem>();

        public GraphRoute(List<GraphRouteItem> items)
        {
            Items = items;
        }

        public GraphRoute(List<string> trail)
        {
            for (int i = 0; i < trail.Count - 1; i++) {
                Items.Add(new GraphRoute.GraphRouteItem() { SourceName = trail[i], TargetName = trail[i + 1] });
            }
        }

        public bool Contains(GraphRouteItem item)
        {
            return Items.Any(x => x.SourceName == item.SourceName && x.TargetName == item.TargetName);
        }

        public GraphRoute ReduceBy(List<GraphRoute> routes)
        {
            return new GraphRoute(Items.Where(z => !ExistsAny(routes, z)).ToList());
        }

        public static bool ExistsAny(List<GraphRoute> routes, GraphRouteItem item)
        {
            return routes.Any(x => x.Contains(item));
        }

    }

    class RouteReducer {

        public static List<GraphRoute> Reduce(List<GraphRoute> routes)
        {
            List<GraphRoute> reduced_routes = new List<GraphRoute>();
            foreach (var route in routes) {
                GraphRoute reduced_route = route.ReduceBy(reduced_routes);
                if (reduced_route.Items.Count > 0) {
                    reduced_routes.Add(reduced_route);
                }
            }
            return reduced_routes;
        }
    }
}
