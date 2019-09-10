namespace SeadQueryCore
{
    //public interface IQuerySetupCompilers {
    //    TabularQuerySetupCompiler DefaultQuerySetupCompiler { get; }
    //    MapQuerySetupCompiler MapQuerySetupCompiler { get; }
    //}

    public interface IResultQueryCompiler {
        string Compile(FacetsConfig2 facetsConfig, ResultConfig resultConfig, string facetCode);
    }
}