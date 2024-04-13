namespace SeadQueryCore
{
    public interface IRangeIntervalSqlCompiler : ISqlCompiler
    {
        string Compile(int interval, int min, int max, int interval_count);
    }
}