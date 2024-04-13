
using System.Collections.Generic;
using System.Data;
using Autofac;

namespace SeadQueryCore;
public interface IFacetTypePlugin
{
    void Register(ContainerBuilder builder);
    
    ICategoryCountSqlCompiler CategoryCountSqlCompiler { get; }
    ICategoryCountService CategoryCountService { get; }
    IFacetContentService FacetContentService { get; }
    IPickFilterCompiler PickFilterCompiler { get; }
    
}

public interface ICategoryCountHelper
{
    public string GetCategory(IDataReader x);
    public int GetCount(IDataReader x);
    public List<decimal> GetExtent(IDataReader x);

    public List<string> GetTables(FacetsConfig2 facetsConfig, CompilePayload payload);
    public List<string> GetFacetCodes(FacetsConfig2 facetsConfig, CompilePayload payload);
    
}
