using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace SeadQueryCore;

public class DiscreteCategoryCountHelper : ICategoryCountHelper
{
    public string GetCategory(IDataReader x)
        => x.Category2String(0);

    public int GetCount(IDataReader x)
        => x.IsDBNull(1) ? 0 : x.GetInt32(1);

    public List<decimal> GetExtent(IDataReader x)
        => [x.IsDBNull(1) ? 0 : x.GetInt32(1)];

    public List<string> GetFacetCodes(FacetsConfig2 facetsConfig, CompilePayload payload)
    {
        if (payload.AggregateFacet == null)
            return facetsConfig.GetFacetCodes();
        return facetsConfig.GetFacetCodes().InsertAt(
            facetsConfig.TargetCode,
            payload.AggregateFacet.FacetCode
        );
    }

    public List<string> GetTables(FacetsConfig2 facetsConfig, CompilePayload payload)
    {
        var tables = payload.TargetFacet.GetResolvedTableNames();
        if (payload.AggregateFacet != null)
            tables = tables.Union(payload.AggregateFacet.GetResolvedTableNames());
        return tables.ToList();
    }

}
