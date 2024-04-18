using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace SeadQueryCore;


public class DiscreteCategoryCountHelper : ICategoryCountHelper
{
    public List<string> GetFacetCodes(FacetsConfig2 facetsConfig, CompilePayload payload)
    {
        if (payload.AggregateFacet == null)
            return facetsConfig.GetFacetCodes();
        return facetsConfig.GetFacetCodes().InsertAt(
            facetsConfig.TargetCode,
            payload.AggregateFacet.FacetCode
        );
    }

    public List<string> GetTables(CompilePayload payload)
    {
        var tables = payload.TargetFacet.GetResolvedTableNames();
        if (payload.AggregateFacet != null)
            tables = tables.Union(payload.AggregateFacet.GetResolvedTableNames());
        return tables.ToList();
    }

}
