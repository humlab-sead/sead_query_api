using SeadQueryCore.Model;

namespace SeadQueryCore
{
    public interface IResultQueryCompiler {
        string Compile(FacetsConfig2 facetsConfig, ResultConfig resultConfig, string facetCode);
    }
}