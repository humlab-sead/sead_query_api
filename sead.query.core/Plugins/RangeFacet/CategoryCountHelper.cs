using System.Collections.Generic;
using System.Data;

namespace SeadQueryCore;

public class RangeCategoryCountHelper : ICategoryCountHelper
{
    public IFacetSetting Config { get; }

    public RangeCategoryCountHelper(IFacetSetting config )
    {   
        Config = config;
    }

    public string GetCategory(IDataReader x)
        => x.IsDBNull(0) ? "(null)" : x.GetString(0);

    public int GetCount(IDataReader x)
        => x.IsDBNull(3) ? 0 : x.GetInt32(3);

    public List<decimal> GetExtent(IDataReader x)
        => [x.IsDBNull(1) ? 0 : x.GetInt32(1), x.IsDBNull(2) ? 0 : x.GetInt32(2)];

    public List<string> GetTables(FacetsConfig2 facetsConfig, CompilePayload payload) => Config.CountTable.WrapToList();
    
    public List<string> GetFacetCodes(FacetsConfig2 facetsConfig, CompilePayload payload) => facetsConfig.GetFacetCodes();
    
}


