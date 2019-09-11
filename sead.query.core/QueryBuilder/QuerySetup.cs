using System;
using System.Collections.Generic;
using static SeadQueryCore.Utility;
using System.Linq;

namespace SeadQueryCore.QueryBuilder
{
    public class QuerySetup {
        public FacetConfig2 TargetConfig;
        public Facet Facet;
        public List<GraphRoute> Routes;
        public List<GraphRoute> ReducedRoutes;

        public List<string> Joins;
        public List<string> Criterias;

        public string CategoryTextFilter { get { return TargetConfig?.TextFilter ?? "";  } }

        public QuerySetup(FacetConfig2 targetConfig, Facet facet, List<string> sqlJoins, Dictionary<string, string> criterias, List<GraphRoute> routes, List<GraphRoute> reducedRoutes)
        {
            TargetConfig = targetConfig;
            Facet = facet;
            Routes = routes;
            ReducedRoutes = reducedRoutes;
            Joins = sqlJoins;
            Criterias = criterias.Select(x => "(" + x.Value + ")").AppendIf(Facet.QueryCriteria).ToList();
        }
    }
}
