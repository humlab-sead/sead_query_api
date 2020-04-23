namespace SeadQueryCore
{
    public interface IResultSqlCompilerLocator
    {
        IResultSqlCompiler Locate(string viewTypeId);
    }
}