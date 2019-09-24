using System;
using System.Collections.Generic;
using static SeadQueryCore.Utility;
using System.Linq;

namespace SeadQueryCore.QueryBuilder
{
    public class QuerySetup {
        public FacetConfig2 TargetConfig { get; set; }
        public Facet Facet { get; set; }
        public List<GraphRoute> Routes { get; set; }

        public List<string> Joins { get; set; }
        public List<string> Criterias { get; set; }

        public string CategoryTextFilter { get { return TargetConfig?.TextFilter ?? "";  } }

        public QuerySetup()
        {
        }

        //public QuerySetup(FacetConfig2 targetConfig, Facet facet, List<string> sqlJoins, Dictionary<string, string> criterias, List<GraphRoute> routes, List<GraphRoute> reducedRoutes)
        //{
        //    TargetConfig = targetConfig;
        //    Facet = facet;
        //    Routes = routes;
        //    Joins = sqlJoins;
        //    Criterias = criterias.Select(x => "(" + x.Value + ")").AppendIf(Facet.QueryCriteria).ToList();
        //}
    }
}
