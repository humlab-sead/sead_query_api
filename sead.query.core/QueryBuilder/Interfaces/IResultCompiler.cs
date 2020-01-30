using SeadQueryCore.Model;

namespace SeadQueryCore
{
    public interface IResultCompiler {
        string Compile(FacetsConfig2 facetsConfig, ResultConfig resultConfig, string facetCode);
    }
}