using System.Collections.Generic;

namespace SeadQueryCore.Plugin.Range;


public class RangeCategoryCountHelper(IFacetSetting config) : IRangeCategoryCountHelper
{
    public IFacetSetting Config { get; } = config;

    public List<string> GetTables(CompilePayload payload) => Config.CountTable.WrapToList();

    public List<string> GetFacetCodes(FacetsConfig2 facetsConfig, CompilePayload payload) => facetsConfig.GetFacetCodes();

}
