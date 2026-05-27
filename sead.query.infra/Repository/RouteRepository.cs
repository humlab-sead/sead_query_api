using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SeadQueryCore;

namespace SeadQueryInfra
{
    public class RouteRepository(RepositoryRegistry registry) : Repository<Route, int>(registry), IRouteRepository
    {
        /// <summary>
        /// Overide that include SourceTable, TargetTable and Steps when calling GetAll
        /// </summary>
        /// <returns></returns>
        protected override IQueryable<Route> GetInclude(IQueryable<Route> query)
        {
            return query.Include(r => r.SourceTable).Include(r => r.TargetTable).Include(r => r.Steps);
        }

        private Dictionary<string, Route> _routeLookup = null!;

        private Dictionary<string, Route> CreateLookup()
        {
            var routes = GetAll();
            var lookup = routes?.ToDictionary(r => r.Name, r => r) ?? [];
            foreach (var route in routes?.Where(r => !string.IsNullOrEmpty(r.Alias)) ?? [])
            {
                lookup.TryAdd(route.Alias!, route);
            }
            return lookup;
        }

        private Dictionary<string, Route> RouteLookup => _routeLookup ??= CreateLookup();

        public Route GetRoute(string key)
        {
            RouteLookup.TryGetValue(key, out var route);
            return route;
        }

        public bool HasRoute(string key)
        {
            return RouteLookup.ContainsKey(key);
        }
    }
}
