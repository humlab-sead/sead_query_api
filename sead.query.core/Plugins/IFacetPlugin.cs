
using System.Collections.Generic;
using Autofac;

namespace SeadQueryCore;
public interface IFacetPlugin
{
    void Register(ContainerBuilder builder);
    ICategoryCountHelper CategoryCountHelper { get; }
    ICategoryCountSqlCompiler CategoryCountSqlCompiler { get; }
    ICategoryInfoService CategoryInfoService { get; }
    ICategoryCountService CategoryCountService { get; }
    IPickFilterCompiler PickFilterCompiler { get; }
}

public interface ICategoryCountHelper
{
    public List<string> GetTables(CompilePayload payload);
    public List<string> GetFacetCodes(FacetsConfig2 facetsConfig, CompilePayload payload);

}
