using System.Collections.Generic;
using System.Data;

namespace SeadQueryCore;

public class RangeCategoryCountHelper(IFacetSetting config) : ICategoryCountHelper
{
    public IFacetSetting Config { get; } = config;

    public List<string> GetTables(CompilePayload payload) => Config.CountTable.WrapToList();

    public List<string> GetFacetCodes(FacetsConfig2 facetsConfig, CompilePayload payload) => facetsConfig.GetFacetCodes();

}


