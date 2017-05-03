using System.Collections.Generic;

namespace QuerySeadDomain.QueryBuilder {
    public interface IQuerySetupBuilder {
        IFacetsGraph Graph { get; set; }

        QuerySetup Build(FacetsConfig2 facetsConfig, string facetCode, List<string> extraTables = null);
        QuerySetup Build(FacetsConfig2 facetsConfig, string facetCode, List<string> extraTables, List<string> facetCodes);
    }
}