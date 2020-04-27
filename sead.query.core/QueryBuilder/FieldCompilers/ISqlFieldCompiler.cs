namespace SeadQueryCore
{
    public interface ISqlFieldCompiler : ISqlCompiler
    {
        string Compile(string expr);
    }
}