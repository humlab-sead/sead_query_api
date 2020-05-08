using System.Collections.Generic;

namespace SeadQueryCore.QueryBuilder
{
    public interface IQuerySetupBuilder
    {
        IFacetsGraph FacetsGraph { get; set; }

        QuerySetup Build(FacetsConfig2 facetsConfig, Facet facet, List<string> extraTables, List<string> facetCodes);
        QuerySetup Build(FacetsConfig2 facetsConfig, Facet facet, IEnumerable<ResultCompositeField> fields);
    }
}