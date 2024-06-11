using System.Collections.Generic;

namespace SeadQueryCore.QueryBuilder
{
    public interface IQuerySetupBuilder
    {
        IRouteFinder RouteFinder { get; set; }

        QuerySetup Build(FacetsConfig2 facetsConfig, Facet facet, List<string> extraTables, List<string> facetCodes);
        QuerySetup Build(FacetsConfig2 facetsConfig, Facet facet, IEnumerable<ResultSpecificationField> fields);
    }
}
