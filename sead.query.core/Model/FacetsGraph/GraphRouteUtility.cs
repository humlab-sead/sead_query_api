﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace SeadQueryCore
{
    public static class GraphRouteUtility
    {
        public static bool ExistsAny(List<GraphRoute> routes, TableRelation item)
        {
            return routes.Any(x => x.Contains(item));
        }

        public static List<GraphRoute> Reduce(List<GraphRoute> routes)
        {
            List<GraphRoute> reduced_routes = new List<GraphRoute>();
            foreach (var route in routes)
            {
                GraphRoute reduced_route = route.ReduceBy(reduced_routes);
                if (reduced_route.Items.Count > 0)
                {
                    reduced_routes.Add(reduced_route);
                }
            }
            return reduced_routes;
        }

        public static string ToString(List<GraphRoute> routes)
        {
            return String.Join("\n", routes.Select(z => $"{routes.IndexOf(z)};{z}"));
        }
    }
}
