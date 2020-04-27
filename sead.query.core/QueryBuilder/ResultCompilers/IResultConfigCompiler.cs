using SeadQueryCore.Model;

namespace SeadQueryCore
{
    public interface IResultConfigCompiler  : ISqlCompiler {
        string Compile(FacetsConfig2 facetsConfig, ResultConfig resultConfig, string facetCode);
    }
}