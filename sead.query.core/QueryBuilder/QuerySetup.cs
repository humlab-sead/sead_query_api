using System.Collections.Generic;

namespace SeadQueryCore.QueryBuilder
{
    public class QuerySetup {
        public FacetConfig2 TargetConfig { get; set; }
        public Facet Facet { get; set; }
        public List<string> Joins { get; set; }
        public List<string> Criterias { get; set; }

        // public string CategoryTextFilter { get { return TargetConfig?.TextFilter ?? "";  } }

        public QuerySetup()
        {
        }

    }
}
